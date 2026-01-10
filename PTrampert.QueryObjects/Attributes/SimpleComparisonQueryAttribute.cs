using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// <see cref="QueryAttribute"/> that builds a simple comparison expression (e.g. <c>==</c>, <c>!=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
    /// </summary>
    public abstract class SimpleComparisonQueryAttribute : QueryAttribute
    {
        /// <summary>
        /// The comparison expression builder to use.
        /// </summary>
        protected virtual Func<Expression, Expression, Expression> ComparisonExpressionBuilder => null;

        /// <summary>
        /// Ignore this query if the property value is null.
        /// </summary>
        public bool IgnoreIfNull { get; set; }

        /// <inheritdoc />
        protected SimpleComparisonQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
            IgnoreIfNull = true;
        }

        /// <inheritdoc />
        public override Expression BuildExpression(object queryObject, PropertyInfo queryProperty, ParameterExpression targetParameter,
            PropertyInfo targetProperty)
        {
            if (queryObject == null) throw new ArgumentNullException(nameof(queryObject));
            if (queryProperty == null) throw new ArgumentNullException(nameof(queryProperty));
            if (targetParameter == null) throw new ArgumentNullException(nameof(targetParameter));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));

            var queryValue = queryProperty.GetValue(queryObject);
            if (IgnoreIfNull && queryValue == null)
                return null;
            var constant = Expression.Constant(queryValue, targetProperty.PropertyType);
            return ComparisonExpressionBuilder(Expression.Property(targetParameter, targetProperty), constant);
        }
    }
}