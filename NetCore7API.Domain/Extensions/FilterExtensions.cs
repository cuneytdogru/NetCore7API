using NetCore7API.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> query, BaseEntityFilter<TEntity> filter, bool ignoreSkipTake = false) where TEntity : class
        {
            query = filter.Apply(query, ignoreSkipTake);
            return query;
        }
    }
}