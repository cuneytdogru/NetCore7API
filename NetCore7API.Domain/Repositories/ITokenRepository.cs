using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Repositories
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<Token?> GetActiveTokenFromUser(Guid userId);

        Task<Token?> GetActiveTokenFromKey(string key);
    }
}