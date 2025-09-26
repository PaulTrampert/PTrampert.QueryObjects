using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is less than the query property.
    ///
    /// Equivalent linq expression: <code>queryable.Where(x => x.TargetProperty &lt; queryObject.QueryProperty)</code>
    /// </summary>
    public class LessThanQueryAttribute : SimpleComparisonQueryAttribute
    {
        public LessThanQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder => Expression.LessThan;
    }
}