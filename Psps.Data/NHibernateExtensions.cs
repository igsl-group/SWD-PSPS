using Psps.Core.Exceptions;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data
{
    public static class NHibernateExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, params string[] properties)
        {
            if (query is NHibernate.Linq.NhQueryable<T>)
            {
                query = NHibernateExtensions.ApplyExpansions(query, properties);
            }
            //    var queryOver = .QueryOver<TEntity>();

            //    var criteria = queryOver.UnderlyingCriteria;

            //    foreach (var property in properties) {
            //        criteria.SetFetchMode(property, FetchMode.Eager);
            //    }

            //    return queryOver;
            return query;
        }

        private static IQueryable<T> ApplyExpansions<T>(IQueryable<T> queryable, IEnumerable<string> expandPaths)
        {
            if (queryable == null) throw new DataLayerException("Query cannot be null");

            var nHibQuery = queryable.Provider as NHibernate.Linq.DefaultQueryProvider;
            if (nHibQuery == null) throw new DataLayerException("Expansion only supported on INHibernateQueryable queries");

            if (!expandPaths.Any()) throw new DataLayerException("Expansion Paths cannot be null");

            var sessionFactory = Psps.Core.Infrastructure.EngineContext.Current.Resolve<ISessionFactory>();
            var currentQueryable = queryable;

            foreach (string expand in expandPaths)
            {
                // We always start with the resulting element type
                var currentType = currentQueryable.ElementType;
                var isFirstFetch = true;
                foreach (string seg in expand.Split('.'))
                {
                    IClassMetadata metadata = sessionFactory.GetClassMetadata(currentType);
                    if (metadata == null)
                    {
                        throw new DataLayerException("Type not recognized as a valid type for this Context");
                    }

                    // Gather information about the property
                    var propInfo = currentType.GetProperty(seg);
                    var propType = propInfo.PropertyType;
                    var metaPropType = metadata.GetPropertyType(seg);

                    // When this is the first segment of a path, we have to use Fetch instead of ThenFetch
                    var propFetchFunctionName = (isFirstFetch ? "Fetch" : "ThenFetch");

                    // The delegateType is a type for the lambda creation to create the correct return value
                    System.Type delegateType;

                    if (metaPropType.IsCollectionType)
                    {
                        // We have to use "FetchMany" or "ThenFetchMany" when the target property is a collection
                        propFetchFunctionName += "Many";

                        // We only support IList<T> or something similar
                        propType = propType.GetGenericArguments().Single();
                        delegateType = typeof(Func<,>).MakeGenericType(currentType,
                                                                        typeof(IEnumerable<>).MakeGenericType(propType));
                    }
                    else
                    {
                        delegateType = typeof(Func<,>).MakeGenericType(currentType, propType);
                    }

                    // Get the correct extension method (Fetch, FetchMany, ThenFetch, or ThenFetchMany)
                    var fetchMethodInfo = typeof(EagerFetchingExtensionMethods).GetMethod(propFetchFunctionName,
                                                                                      BindingFlags.Static |
                                                                                      BindingFlags.Public |
                                                                                      BindingFlags.InvokeMethod);
                    var fetchMethodTypes = new List<System.Type>();
                    fetchMethodTypes.AddRange(currentQueryable.GetType().GetGenericArguments().Take(isFirstFetch ? 1 : 2));
                    fetchMethodTypes.Add(propType);
                    fetchMethodInfo = fetchMethodInfo.MakeGenericMethod(fetchMethodTypes.ToArray());

                    // Create an expression of type new delegateType(x => x.{seg.Name})
                    var exprParam = System.Linq.Expressions.Expression.Parameter(currentType, "x");
                    var exprProp = System.Linq.Expressions.Expression.Property(exprParam, seg);
                    var exprLambda = System.Linq.Expressions.Expression.Lambda(delegateType, exprProp,
                                                                               new System.Linq.Expressions.
                                                                                   ParameterExpression[] { exprParam });

                    // Call the *Fetch* function
                    var args = new object[] { currentQueryable, exprLambda };
                    currentQueryable = (IQueryable)fetchMethodInfo.Invoke(null, args) as IQueryable<T>;

                    currentType = propType;
                    isFirstFetch = false;
                }
            }

            return currentQueryable;
        }
    }
}