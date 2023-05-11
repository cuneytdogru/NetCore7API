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
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsUserNameInUse(string userName);

        Task<bool> IsUserNameInUse(string userName, Guid except);

        Task<bool> IsEmailInUse(string email);

        Task<bool> IsEmailInUse(string email, Guid except);

        Task<User?> GetByUserNameAsync(string userName);
    }
}