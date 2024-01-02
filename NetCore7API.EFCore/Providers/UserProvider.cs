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
                .Include(x => x.User)
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

        public async Task<PagedResponse<CommentDto, CommentFilter>> GetProfileCommentsAsync(CommentFilter filter)
        {
            var comments = await Context.Set<Comment>()
                .OrderByDescending(x => x.CreatedDate)
                .ApplyFilter(filter)
                .Include(x => x.User)
                .AsNoTracking()
                .ToListAsync();

            var totalCount = await Context.Set<Comment>()
                .ApplyFilter(filter, true)
                .CountAsync();

            return new PagedResponse<CommentDto, CommentFilter>(
                _mapper.Map<IEnumerable<CommentDto>>(comments),
                filter,
                totalCount);
        }
    }
}