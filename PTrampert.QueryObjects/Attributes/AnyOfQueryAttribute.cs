using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is contained in the query property's collection.
    /// May only be used on query properties that implement <see cref="IEnumerable"/>.
    /// When the target property is also a collection, checks if there is any overlap
    /// between the two collections.
    /// </summary>
    public class AnyOfQueryAttribute : QueryAttribute
    {
        /// <inheritdoc />
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

            var targetElementType = targetProperty.PropertyType;
            if (typeof(string) != targetElementType 
                && typeof(IEnumerable).IsAssignableFrom(targetElementType))
            {
                targetElementType = targetElementType.GetCollectionElementType();
                return BuildCollectionExpression(queryValue, targetParameter, targetProperty, targetElementType);
            }
            var containsMethod = targetElementType.GetContainsMethod();

            var constant = Expression.Constant(queryValue);
            var propertyAccess = Expression.Property(targetParameter, targetProperty);
            return Expression.Call(containsMethod, constant, propertyAccess);
        }
        
        private Expression BuildCollectionExpression(
            IEnumerable queryValue, 
            ParameterExpression targetParameter,
            PropertyInfo targetProperty,
            Type targetElementType
        )
        {
            var intersectMethod = targetElementType.GetIntersectMethod();
            var anyMethod = targetElementType.GetAnyMethod();
            var constant = Expression.Constant(queryValue);
            var propertyAccess = Expression.Property(targetParameter, targetProperty);
            var intersectCall = Expression.Call(intersectMethod, propertyAccess, constant);
            var anyCall = Expression.Call(anyMethod, intersectCall);
            return anyCall;
        }
    }
}