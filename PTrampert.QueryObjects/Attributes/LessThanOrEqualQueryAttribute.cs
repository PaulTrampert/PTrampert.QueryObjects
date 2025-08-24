using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is less than or equal to the query property.
    /// </summary>
    public class LessThanOrEqualQueryAttribute : SimpleComparisonQueryAttribute
    {
        public LessThanOrEqualQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get { return Expression.LessThanOrEqual; } }
    }
}