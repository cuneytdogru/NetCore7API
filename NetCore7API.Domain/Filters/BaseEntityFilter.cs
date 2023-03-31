using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Filters
{
    public abstract class BaseEntityFilter<TEntity> : BaseFilter where TEntity : class
    {
        public virtual IQueryable<TEntity> Apply(IQueryable<TEntity> query, bool ignoreSkipTake = false)
        {
            if (!string.IsNullOrWhiteSpace(this.OrderBy))
                query = query.OrderBy(OrderBy);

            if (!ignoreSkipTake && this.Skip.HasValue)
                query = query.Skip((int)this.Skip);

            if (!ignoreSkipTake && this.Take.HasValue)
                query = query.Take((int)this.Take!);

            return query;
        }
    }
}