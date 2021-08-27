using Psps.Core.JqGrid.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Psps.Core.JqGrid.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Orders the sequence by specific column and direction.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="direction">desc or else.</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumn, string direction) where T : class
        {
            ParameterExpression rootParameter = Expression.Parameter(typeof(T), "r");
            var columnArr = string.Format("{0} {1}", sortColumn.Trim(), direction.Trim()).SplitAndTrim(',').ToArray();

            var sortArr = columnArr[0].Split(' ');
            string methodName = string.Format("OrderBy{0}", sortArr.Count() > 1 ? sortArr[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "Descending" : ""
                                                                                : direction.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "Descending" : "");
            query = query.Provider.CreateQuery<T>(GenerateMethodCall<T>(query, methodName, sortArr[0]));

            foreach (var c in columnArr.Skip(1))
            {
                sortArr = c.Split(' ');
                methodName = string.Format("ThenBy{0}", sortArr.Count() > 1 ? sortArr[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "Descending" : ""
                                                                            : direction.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "Descending" : "");
                query = query.Provider.CreateQuery<T>(GenerateMethodCall<T>(query, methodName, sortArr[0]));
            }

            return query;
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, Filter filter) where T : class
        {
            var filters = filter.BuildExpression<T>();
            return filters != null ? query.Where(filters) : query;
        }

        #region Helper

        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type resultType) where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");
            // create the selector part, but support child properties
            Expression propertyAccess = propertyName.Split('.').Aggregate<string, MemberExpression>(null, (current, property) => Expression.Property(current ?? (parameter as Expression), property));
            resultType = propertyAccess.Type;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                                            new Type[] { type, selectorResultType },
                                            source.Expression, Expression.Quote(selector));
            return resultExp;
        }

        #endregion Helper
    }
}