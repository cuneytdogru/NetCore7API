using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.EFCore.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Creates global filter for given Interface
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="expression"></param>
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
              Expression<Func<TInterface, bool>> expression)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterface(typeof(TInterface).FullName) != null)
                {
                    var parameterType = Expression.Parameter(entityType.ClrType);

                    var expressionFilter = ReplacingExpressionVisitor.Replace(
                        expression.Parameters.Single(), parameterType, expression.Body);

                    var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);

                    if (entityTypeBuilder.Metadata.GetQueryFilter() != null)
                    {
                        var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
                        var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                            currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                        expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
                    }

                    var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
                    entityTypeBuilder.HasQueryFilter(lambdaExpression);
                }
            }
        }
    }
}