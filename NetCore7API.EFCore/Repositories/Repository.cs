using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.EFCore.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly Context.BlogContext Context;

        public Repository(Context.BlogContext context)
        {
            this.Context = context;
        }

        public IQueryable<TEntity> All()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> AllAsNoTracking()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> All(params Expression<Func<TEntity, object>>[] includes)
        {
            var baseQuery = Context.Set<TEntity>().AsQueryable();

            foreach (var include in includes)
                baseQuery = baseQuery.Include(include);

            return baseQuery;
        }

        public async ValueTask<TEntity?> FindAsync(object key)
        {
            return await Context.Set<TEntity>().FindAsync(key);
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            return await Context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
        {
            var baseQuery = Context.Set<TEntity>().AsQueryable();

            foreach (var include in includes)
                baseQuery = baseQuery.Include(include);

            return await baseQuery.SingleOrDefaultAsync(a => a.Id == id);
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            TEntity existingEntity = Context.Set<TEntity>().Find(entity.Id);

            if (existingEntity == null)
                throw new DbUpdateException($"Entity with unique id '{entity.Id}' not found.");

            Context.Entry(existingEntity).CurrentValues.SetValues(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                this.Update(entity);
        }

        public virtual void SoftDelete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void SoftDeleteRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TEntity>> ListAsync(BaseEntityFilter<TEntity> filter)
        {
            return await Context.Set<TEntity>().ApplyFilter(filter).ToListAsync();
        }

        public async Task<int> TotalCountAsync(BaseEntityFilter<TEntity> filter)
        {
            return await Context.Set<TEntity>().ApplyFilter(filter, true).CountAsync();
        }
    }
}