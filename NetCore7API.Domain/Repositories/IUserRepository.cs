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
        /// <summary>
        /// Finds an user with username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User?> GetByUserNameAsync(string userName);

        /// <summary>
        /// Finds an user with username or email.
        /// </summary>
        /// <returns></returns>
        Task<User?> GetByUserNameOrEmailAsync(string value);
    }
}