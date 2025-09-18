using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Perform a Contains query on the applied property.
    /// </summary>
    public class StringContainsQueryAttribute : QueryAttribute
    {
        public StringContainsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }
        
        /// <summary>
        /// Ignore the search term if not included.
        /// </summary>
        public bool IgnoreIfNull { get; set; }

        /// <inheritdoc />
        public override Expression BuildExpression(object queryObject, PropertyInfo queryProperty, ParameterExpression targetParameter,
            PropertyInfo targetProperty)
        {
            if (queryObject == null) throw new ArgumentNullException(nameof(queryObject));
            if (queryProperty == null) throw new ArgumentNullException(nameof(queryProperty));
            if (targetParameter == null) throw new ArgumentNullException(nameof(targetParameter));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));

            var queryValue = queryProperty.GetValue(queryObject);
            if (queryValue == null)
                return IgnoreIfNull ? null : Expression.Constant(false);
            var constant = Expression.Constant(queryValue);
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
            return Expression.Call(Expression.Property(targetParameter, targetProperty), containsMethod, constant);
        }
    }
}