using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    public class AnyOfAttribute : QueryAttribute
    {
        public AnyOfAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }
        
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