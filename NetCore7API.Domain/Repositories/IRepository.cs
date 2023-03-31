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
        ValueTask<T?> FindAsync(object key);

        /// <summary>
        /// Gets an entity from database with unique id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetAsync(Guid id);

        /// <summary>
        /// Get an entity from database with unique id and defined includes.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<T?> GetAsync(Guid id, params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> ListAsync(BaseEntityFilter<T> filter);

        Task<int> TotalCountAsync(BaseEntityFilter<T> filter);

        /// <summary>
        /// Gets all records with AsNoTracking.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AllAsNoTracking();

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// Gets all records with with includes
        /// </summary>
        /// <param name="includes">Entities to include</param>
        /// <returns></returns>
        IQueryable<T> All(params Expression<Func<T, object>>[] includes);
    }
}