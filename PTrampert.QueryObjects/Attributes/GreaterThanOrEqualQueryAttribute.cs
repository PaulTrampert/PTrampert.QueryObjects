using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is greater than or equal to the query property.
    /// </summary>
    public class GreaterThanOrEqualQueryAttribute : SimpleComparisonQueryAttribute
    {
        public GreaterThanOrEqualQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get { return Expression.GreaterThanOrEqual; } }
    }
}