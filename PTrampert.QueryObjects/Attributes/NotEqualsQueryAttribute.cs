using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property does not equal the query property.
    /// </summary>
    public class NotEqualsQueryAttribute : SimpleComparisonQueryAttribute
    {
        public NotEqualsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get { return Expression.NotEqual; } }
    }
}