using NetCore7API.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Repositories
{
    public interface IRepository<T> where T : Models.Interfaces.BaseEntity
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void SoftDelete(T entity);

        void SoftDeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Finds an entity with primary key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ValueTask<T?> FindAsync(Guid id);

        /// <summary>
        /// Gets an entity from database with unique id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetAsync(Guid id);

        Task<IEnumerable<T>> ListAsync(BaseEntityFilter<T> filter);

        Task<int> TotalCountAsync(BaseEntityFilter<T> filter);
    }
}