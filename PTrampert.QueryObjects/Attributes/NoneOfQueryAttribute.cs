using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is not contained in the query property's enumerable value.
    /// If the target property is also a collection, checks if none of the target property's values
    /// are contained in the query property's enumerable value.
    /// </summary>
    public class NoneOfQueryAttribute : AnyOfQueryAttribute
    {
        /// <inheritdoc />
        public NoneOfQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }
        
        /// <inheritdoc />
        public override Expression BuildExpression(object queryObject, PropertyInfo queryProperty, ParameterExpression targetParameter,
            PropertyInfo targetProperty)
        {
            var result = base.BuildExpression(queryObject, queryProperty, targetParameter, targetProperty);
            return result == null ? null : Expression.Not(result);
        }
    }
}