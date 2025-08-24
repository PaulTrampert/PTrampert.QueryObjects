using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Checks if the target property is greater than the query property.
/// </summary>
public class GreaterThanQueryAttribute(string? targetProperty = null) : SimpleComparisonQueryAttribute(targetProperty)
{
    /// <inheritdoc />
    protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get; } = Expression.GreaterThan;
}