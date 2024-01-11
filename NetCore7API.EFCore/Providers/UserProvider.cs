using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs.User;
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

namespace NetCore7API.EFCore.Providers
{
    public class UserProvider : Provider, IUserProvider
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserProvider(BlogContext context, ITokenService tokenService, IMapper mapper) : base(context)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<bool> IsUserNameInUse(string userName, Guid except)
        {
            return await Context.Set<User>()
                .Where(x => x.Id != except)
                .AnyAsync(x => x.UserName == userName);
        }

        public async Task<bool> IsEmailInUse(string email, Guid except)
        {
            return await Context.Set<User>()
                .Where(x => x.Id != except)
                .AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsUserNameInUse(string userName)
        {
            return await this.IsUserNameInUse(userName, Guid.Empty);
        }

        public async Task<bool> IsEmailInUse(string email)
        {
            return await this.IsEmailInUse(email, Guid.Empty);
        }

        public async Task<UserDto?> GetUserAsync(Guid id)
        {
            var user = await Context.Set<User>()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<UserDto?>(user);
        }

        public async Task<ProfileDto?> GetProfileByUserNameAsync(string userName)
        {
            var user = await Context.Set<User>()
                .FirstOrDefaultAsync(x => x.UserName == userName);

            var profile = _mapper.Map<ProfileDto?>(user);

            if (user is not null)
                profile.TotalPosts = await Context.Set<Post>()
                    .Where(x => x.UserId == user.Id)
                    .CountAsync();

            return profile;
        }

        public async Task<PagedResponse<PostDto, PostFilter>> GetProfilePostsAsync(PostFilter filter)
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

        public async Task<PagedResponse<CommentDto, CommentFilter>> GetProfileCommentsAsync(CommentFilter filter)
        {
            var comments = await Context.Set<Comment>()
                .OrderByDescending(x => x.CreatedDate)
                .ApplyFilter(filter)
                .AsNoTracking()
                .Select(x => new CommentDto()
                {
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    PostId = x.PostId,
                    ResponseToUser = new PublicUserDto()
                    {
                        FullName = x.Post.User.FullName,
                        Id = x.Post.User.Id,
                        UserName = x.Post.User.UserName,
                    },
                    Text = x.Text,
                    User = new PublicUserDto()
                    {
                        FullName = x.User.FullName,
                        Id = x.User.Id,
                        UserName = x.User.UserName,
                    },
                    UserId = x.UserId
                })
                .ToListAsync();

            var totalCount = await Context.Set<Comment>()
                .ApplyFilter(filter, true)
                .CountAsync();

            return new PagedResponse<CommentDto, CommentFilter>(
                comments,
                filter,
                totalCount);
        }
    }
}