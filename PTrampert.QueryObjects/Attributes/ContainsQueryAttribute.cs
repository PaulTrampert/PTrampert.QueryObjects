using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the query property is contained within the target property.
    /// For example, <code>targetQueryable.Where(q =&gt; q.TargetProperty.Contains(queryObject.QueryProperty))</code>
    /// </summary>
    public class ContainsQueryAttribute : QueryAttribute
    {
        public bool IgnoreIfNull { get; set; } = true;

        public ContainsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }
        
        public override Expression BuildExpression(object queryObject, PropertyInfo queryProperty, ParameterExpression targetParameter,
            PropertyInfo targetProperty)
        {
            if (queryObject == null) throw new ArgumentNullException(nameof(queryObject));
            if (queryProperty == null) throw new ArgumentNullException(nameof(queryProperty));
            if (targetParameter == null) throw new ArgumentNullException(nameof(targetParameter));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));

            var queryValue = queryProperty.GetValue(queryObject) as IEnumerable;
            if (IgnoreIfNull && queryValue == null)
                return null;
            var constant = Expression.Constant(queryValue);
            var containsMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(queryProperty.PropertyType);
            return Expression.Call(containsMethod, Expression.Property(targetParameter, targetProperty), constant);
        }
    }
}