using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks the target property for equality with the query property.
    ///
    /// Equivalent linq expression: <code>queryable.Where(x => x.TargetProperty == queryObject.QueryProperty)</code>
    /// </summary>
    public class EqualsQueryAttribute : SimpleComparisonQueryAttribute
    {
        /// <inheritdoc />
        public EqualsQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder => Expression.Equal;
    }
}