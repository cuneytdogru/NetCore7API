﻿using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Repositories;
using NetCore7API.EFCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.EFCore.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BlogContext context) : base(context)
        {
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(x => x.UserName == userName);
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
    }
}