using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Internals
{
    /// <summary>
    /// Marker interface for query expression builders.
    /// </summary>
    internal interface IQueryExpressionBuilder {}

    /// <summary>
    /// Builds a query expression from a query object.
    /// </summary>
    /// <typeparam name="TTarget">The type being queried or filtered.</typeparam>
    internal class QueryExpressionBuilder<TTarget> : IQueryExpressionBuilder
    {
        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TTarget));
        
        private readonly ConcurrentDictionary<Type, IEnumerable<QueryPropertyMapping>> _propertyMappings = new();
        
        /// <summary>
        /// Builds a query expression from a query object.
        /// </summary>
        /// <param name="queryObject">The query object to build the expression from.</param>
        /// <returns>The resulting query expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown if queryObject is null.</exception>
        public Expression<Func<TTarget, bool>> BuildQueryExpression(object queryObject)
        {
            if (queryObject == null) throw new ArgumentNullException(nameof(queryObject));
            var queryType = queryObject.GetType();
            var propertyMappings = GetPropertyMappings(queryType);
            var clauses = propertyMappings
                .Select(m => m.QueryAttribute.BuildExpression(queryObject, m.QueryProperty, _parameter, m.TargetProperty))
                .Where(c => c != null)
                .ToList();

            if (queryObject is IQueryObject<TTarget> queryObjectWithCustomExpression)
            {
                var query = queryObjectWithCustomExpression.BuildQueryExpression();
                if (query != null)
                {
                    var visitor = new ParameterReplacementVisitor(query.Parameters[0], _parameter);
                    query = visitor.Visit(query) as Expression<Func<TTarget, bool>>;
                    clauses.Add(query.Body);
                }
            }

            var clausesArray = clauses.ToArray();
            var expression = clausesArray.Any()
                ? clausesArray.Aggregate(Expression.AndAlso)
                : Expression.Constant(true);
            while (expression.CanReduce)
            {
                expression = expression.Reduce();
            }
            return Expression.Lambda<Func<TTarget, bool>>(expression, _parameter);
        }
        
        private IEnumerable<QueryPropertyMapping> GetPropertyMappings(Type queryType)
        {
            return _propertyMappings.GetOrAdd(queryType, qType =>
            {
                var mappings = new List<QueryPropertyMapping>();
                var queryProperties = qType.GetProperties();
                foreach (var queryProperty in queryProperties)
                {
                    var attribute = queryProperty.GetCustomAttribute<QueryAttribute>();
                    if (attribute == null) continue;
                    var targetPropertyName = attribute.TargetProperty ?? queryProperty.Name;
                    var targetProperty = typeof(TTarget).GetProperty(targetPropertyName);
                    if (targetProperty == null) continue;
                    mappings.Add(new QueryPropertyMapping
                    {
                        QueryProperty = queryProperty,
                        TargetProperty = targetProperty,
                        QueryAttribute = attribute
                    });
                }
                return mappings;
            });
        }
    }

}