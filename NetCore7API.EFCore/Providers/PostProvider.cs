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
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == commentId);

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<PagedResponse<PostDto, PostFilter>> ListBlogFeedAsync(PostFilter filter)
        {
            var posts = await Context.Set<Post>()
                .OrderByDescending(x => x.CreatedDate)
                .ApplyFilter(filter)
                .Include(x => x.User)
                .Include(x => x.Comments.OrderByDescending(x => x.CreatedDate).Take(1))
                    .ThenInclude(c => c.User)
                .Include(x => x.Likes.Where(x => x.UserId == _tokenService.UserId).Take(1))
                .AsNoTracking()
                .ToListAsync();

            var totalCount = await Context.Set<Post>()
                .ApplyFilter(filter, true)
                .CountAsync();

            return new PagedResponse<PostDto, PostFilter>(
                _mapper.Map<IEnumerable<PostDto>>(posts),
                filter,
                totalCount);
        }

        public async Task<PostDto?> GetPostDetailAsync(Guid id)
        {
            var post = await Context.Set<Post>()
                .Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.Comments.OrderByDescending(x => x.CreatedDate).Take(5))
                    .ThenInclude(c => c.User)
                .Include(x => x.Likes.Where(x => x.UserId == _tokenService.UserId).Take(1))
                .AsNoTracking()
                .FirstOrDefaultAsync();

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