using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Checks if the target property is less than or equal to the query property.
/// </summary>
public class LessThanOrEqualQueryAttribute(string? targetProperty  = null) : SimpleComparisonQueryAttribute(targetProperty)
{
    /// <inheritdoc />
    protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get; } = Expression.LessThanOrEqual;
}