using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is contained in the query property's collection.
    /// May only be used on query properties that implement <see cref="IEnumerable"/>
    /// </summary>
    public class AnyOfQueryAttribute : QueryAttribute
    {
        public AnyOfQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }
        
        /// <inheritdoc />
        public override Expression BuildExpression(
            object queryObject, 
            PropertyInfo queryProperty, 
            ParameterExpression targetParameter,
            PropertyInfo targetProperty
        )
        {
            if (!(queryProperty.GetValue(queryObject) is IEnumerable queryValue))
                return null;

            var elementType = targetProperty.PropertyType;
            var containsMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(elementType);

            var constant = Expression.Constant(queryValue);
            var propertyAccess = Expression.Property(targetParameter, targetProperty);
            return Expression.Call(containsMethod, constant, propertyAccess);
        }
    }
}