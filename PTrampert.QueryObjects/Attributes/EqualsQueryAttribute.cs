using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks the target property for equality with the query property.
    /// </summary>
    public class EqualsQueryAttribute : SimpleComparisonQueryAttribute
    {
        public EqualsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get { return Expression.Equal; } }
    }
}