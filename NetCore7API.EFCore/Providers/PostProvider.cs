using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Providers;
using NetCore7API.Domain.Services;
using NetCore7API.EFCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetCore7API.EFCore.Providers
{
    public class PostProvider : Provider, IPostProvider
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public PostProvider(BlogContext context, ITokenService tokenService, IMapper mapper) : base(context)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<CommentDto?> GetCommentAsync(Guid commentId)
        {
            var comment = await Context.Set<Comment>()
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == commentId);

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<PagedResponse<PostDto, PostFilter>> ListBlogFeedAsync(PostFilter filter)
        {
            var posts = await Context.Set<Post>()
                .OrderByDescending(x => x.CreatedDate)
                .ApplyFilter(filter)
                .AsNoTracking()
                .Select(post => new PostDto()
                {
                    Id = post.Id,
                    CreatedBy = post.CreatedBy,
                    CreatedDate = post.CreatedDate,
                    ImageURL = post.ImageURL,
                    Text = post.Text,
                    TotalLikes = post.TotalLikes,
                    TotalComments = post.TotalComments,
                    IsLiked = post.Likes.Any(x => x.UserId == _tokenService.UserId),
                    UserId = post.UserId,
                    User = new Domain.DTOs.User.PublicUserDto()
                    {
                        Id = post.UserId,
                        FullName = post.User.FullName,
                        UserName = post.User.UserName,
                    },
                    Comments = post.Comments
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(1)
                    .Select(comment => new CommentDto()
                    {
                        Id = comment.Id,
                        Text = comment.Text,
                        PostId = comment.PostId,
                        CreatedDate = comment.CreatedDate,
                        CreatedBy = comment.CreatedBy,
                        ModifiedBy = comment.ModifiedBy,
                        ModifiedDate = comment.ModifiedDate,
                        UserId = comment.UserId,
                        User = new Domain.DTOs.User.PublicUserDto()
                        {
                            Id = comment.UserId,
                            FullName = comment.User.FullName,
                            UserName = comment.User.UserName,
                        }
                    }),
                })
                .ToListAsync();

            var totalCount = await Context.Set<Post>()
                .ApplyFilter(filter, true)
                .CountAsync();

            return new PagedResponse<PostDto, PostFilter>(
                posts,
                filter,
                totalCount);
        }

        public async Task<PostDto?> GetPostDetailAsync(Guid id)
        {
            var post = await Context.Set<Post>()
                .OrderByDescending(x => x.CreatedDate)
                .AsNoTracking()
                .Select(post => new PostDto()
                {
                    Id = post.Id,
                    CreatedBy = post.CreatedBy,
                    CreatedDate = post.CreatedDate,
                    ImageURL = post.ImageURL,
                    Text = post.Text,
                    TotalLikes = post.TotalLikes,
                    TotalComments = post.TotalComments,
                    IsLiked = post.Likes.Any(x => x.UserId == _tokenService.UserId),
                    UserId = post.UserId,
                    User = new Domain.DTOs.User.PublicUserDto()
                    {
                        Id = post.UserId,
                        FullName = post.User.FullName,
                        UserName = post.User.UserName,
                    },
                    Comments = post.Comments
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(5)
                    .Select(comment => new CommentDto()
                    {
                        Id = comment.Id,
                        Text = comment.Text,
                        PostId = comment.PostId,
                        CreatedDate = comment.CreatedDate,
                        CreatedBy = comment.CreatedBy,
                        ModifiedBy = comment.ModifiedBy,
                        ModifiedDate = comment.ModifiedDate,
                        UserId = comment.UserId,
                        User = new Domain.DTOs.User.PublicUserDto()
                        {
                            Id = comment.UserId,
                            FullName = comment.User.FullName,
                            UserName = comment.User.UserName,
                        }
                    }),
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PagedResponse<CommentDto, CommentFilter>> ListCommentsAsync(Guid id, CommentFilter filter)
        {
            var comments = await Context.Set<Comment>()
                .Where(x => x.PostId == id)
                .Include(c => c.User)
                .OrderByDescending(x => x.CreatedDate)
                .ApplyFilter(filter)
                .AsNoTracking()
                .ToListAsync();

            var totalCount = await Context.Set<Comment>()
                .Where(x => x.PostId == id)
                .ApplyFilter(filter, true)
                .CountAsync();

            return new PagedResponse<CommentDto, CommentFilter>(
                _mapper.Map<IEnumerable<CommentDto>>(comments),
                filter,
                totalCount);
        }
    }
}