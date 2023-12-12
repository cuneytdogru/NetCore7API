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

        private TEntity? GetLocalEntity(Guid id)
        {
            return Context.Set<TEntity>().Local.SingleOrDefault(x => x.Id == id);
        }

        public async ValueTask<TEntity?> FindAsync(Guid id)
        {
            var entity = this.GetLocalEntity(id);

            if (entity is not null)
                return entity;

            return await GetAsync(id);
        }

        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            return await Context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
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