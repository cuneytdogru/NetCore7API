using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Utils;
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

        public async Task<User?> GetByUserNameOrEmailAsync(string value)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(x => x.UserName == value || x.Email == value);
        }
    }
}