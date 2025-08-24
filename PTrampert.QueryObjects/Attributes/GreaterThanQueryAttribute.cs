using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is greater than the query property.
    /// </summary>
    public class GreaterThanQueryAttribute : SimpleComparisonQueryAttribute
    {
        public GreaterThanQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder => Expression.GreaterThan;
    }
}