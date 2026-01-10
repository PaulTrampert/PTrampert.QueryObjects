using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is greater than or equal to the query property.
    ///
    /// Equivalent linq expression: <code>queryable.Where(x => x.TargetProperty &gt;= queryObject.QueryProperty)</code>
    /// </summary>
    public class GreaterThanOrEqualQueryAttribute : SimpleComparisonQueryAttribute
    {
        /// <inheritdoc />
        public GreaterThanOrEqualQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder => Expression.GreaterThanOrEqual;
    }
}