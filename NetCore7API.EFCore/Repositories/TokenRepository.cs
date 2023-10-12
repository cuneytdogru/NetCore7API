using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs.Post;
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
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        public TokenRepository(BlogContext context) : base(context)
        {
        }

        public async Task<Token?> GetActiveTokenFromUser(Guid userId)
        {
            return await Context.Set<Token>()
                .Where(x => x.UserId == userId)
                .Where(x => x.Expires > DateTime.UtcNow)
                .OrderByDescending(x => x.Expires)
                .FirstOrDefaultAsync();
        }

        public async Task<Token?> GetActiveTokenFromKey(string key)
        {
            return await Context.Set<Token>()
                .Where(x => x.Key == key)
                .Where(x => x.Expires > DateTime.UtcNow)
                .SingleOrDefaultAsync();
        }
    }
}