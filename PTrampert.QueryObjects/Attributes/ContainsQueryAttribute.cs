using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the query property is contained within the target property.
    /// For example, <code>targetQueryable.Where(q =&gt; q.TargetProperty.Contains(queryObject.QueryProperty))</code>
    ///
    /// Use <see cref="StringContainsQueryAttribute"/> for <see cref="string.Contains"/>.
    /// </summary>
    public class ContainsQueryAttribute : QueryAttribute
    {
        /// <summary>
        /// If the query property is null, ignore this filter. Defaults to true.
        /// </summary>
        public bool IgnoreIfNull { get; set; } = true;

        /// <inheritdoc />
        public ContainsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
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

            if (!typeof(IEnumerable).IsAssignableFrom(targetProperty.PropertyType))
            {
                throw new InvalidOperationException($"The target property '{targetProperty.Name}' is not a collection type.");
            }

            var elementType = targetProperty.PropertyType.GetCollectionElementType();
            var constant = Expression.Constant(queryValue);
            var containsMethod = elementType.GetContainsMethod();
            return Expression.Call(containsMethod, Expression.Property(targetParameter, targetProperty), constant);
        }
    }
}