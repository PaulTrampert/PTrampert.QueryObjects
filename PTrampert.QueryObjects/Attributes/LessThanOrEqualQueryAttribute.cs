using System;
using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Checks if the target property is less than or equal to the query property.
    ///
    /// Equivalent linq expression: <code>queryable.Where(x => x.TargetProperty &lt;= queryObject.QueryProperty)</code>
    /// </summary>
    public class LessThanOrEqualQueryAttribute : SimpleComparisonQueryAttribute
    {
        /// <inheritdoc />
        public LessThanOrEqualQueryAttribute(string targetProperty = null)
            : base(targetProperty)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder => Expression.LessThanOrEqual;
    }
}