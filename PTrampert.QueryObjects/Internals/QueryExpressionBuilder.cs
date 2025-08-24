using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Internals
{
    /// <summary>
    /// Builds a query expression from a query object.
    /// </summary>
    /// <typeparam name="TTarget">The type being queried or filtered.</typeparam>
    internal class QueryExpressionBuilder<TTarget>
    {
        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TTarget));

        /// <summary>
        /// Builds a query expression from a query object.
        /// </summary>
        /// <param name="queryObject">The query object to build the expression from.</param>
        /// <returns>The resulting query expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown if queryObject is null.</exception>
        public Expression<Func<TTarget, bool>> BuildQueryExpression(object queryObject)
        {
            if (queryObject == null) throw new ArgumentNullException(nameof(queryObject));
            var queryProperties = queryObject.GetType().GetProperties();
            var clauses = new List<Expression>();
            foreach (var queryProperty in queryProperties)
            {
                var attribute = queryProperty.GetCustomAttribute<QueryAttribute>();
                if (attribute == null) continue;
                var targetPropertyName = attribute.TargetProperty ?? queryProperty.Name;
                var targetProperty = typeof(TTarget).GetProperty(targetPropertyName);
                if (targetProperty == null) continue;
                var clause = attribute.BuildExpression(queryObject, queryProperty, _parameter, targetProperty);
                if (clause != null)
                    clauses.Add(clause);
            }

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

            var expression = clauses.Any()
                ? clauses.Aggregate(Expression.AndAlso)
                : Expression.Constant(true);
            while (expression.CanReduce)
            {
                expression = expression.Reduce();
            }
            return Expression.Lambda<Func<TTarget, bool>>(expression, _parameter);
        }
    }

}