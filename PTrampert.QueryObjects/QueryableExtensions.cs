using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/> and <see cref="IEnumerable{T}"/> to filter based on query objects.
    /// </summary>
    public static class QueryableExtensions
    {
        private static readonly ConcurrentDictionary<Type, IQueryExpressionBuilder> ExpressionBuilders = new();
        
        /// <summary>
        /// Filters a queryable based on the properties of a query object. By default, properties are matched by name, and
        /// only properties annotated with <see cref="Attributes.QueryAttribute"/> are included in the filter. This behavior
        /// can be extended by implementing <see cref="IQueryObject{T}"/> on the query object.
        /// </summary>
        /// <param name="queryable">The queryable to be fitered.</param>
        /// <param name="queryObject">The query object used to build the filter.</param>
        /// <typeparam name="T">The type being filtered.</typeparam>
        /// <typeparam name="TQuery">The query object type.</typeparam>
        /// <returns>The filtered <see cref="IQueryable{T}"/></returns>
        public static IQueryable<T> Where<T, TQuery>(this IQueryable<T> queryable, TQuery queryObject)
        {
            var queryableType = typeof(T);
            var expressionBuilder = (QueryExpressionBuilder<T>)ExpressionBuilders
                .GetOrAdd(queryableType, _ => new QueryExpressionBuilder<T>());
            var expression = expressionBuilder.BuildQueryExpression(queryObject);
            return Queryable.Where(queryable, expression);
        }

        /// <summary>
        /// Filters a enumerable based on the properties of a query object. By default, properties are matched by name, and
        /// only properties annotated with <see cref="Attributes.QueryAttribute"/> are included in the filter. This behavior
        /// can be extended by implementing <see cref="IQueryObject{T}"/> on the query object.
        /// </summary>
        /// <param name="enumerable">The enumerable to be fitered.</param>
        /// <param name="queryObject">The query object used to build the filter.</param>
        /// <typeparam name="T">The type being filtered.</typeparam>
        /// <typeparam name="TQuery">The query object type.</typeparam>
        /// <returns>The filtered <see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> Where<T, TQuery>(this IEnumerable<T> enumerable, TQuery queryObject)
        {
            return enumerable.AsQueryable().Where(queryObject);
        }
    }
}