using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property does not equal the query property.
    ///
    /// Equivalent linq expression: <code>queryable.Where(x => x.TargetProperty != queryObject.QueryProperty)</code>
    /// </summary>
    public class NotEqualsQueryAttribute : SimpleComparisonQueryAttribute
    {
        public NotEqualsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder => Expression.NotEqual;
    }
}