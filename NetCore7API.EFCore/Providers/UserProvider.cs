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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

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
    }
}