using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Checks if the target property is less than the query property.
/// </summary>
public class LessThanQueryAttribute(string? targetProperty = null) : SimpleComparisonQueryAttribute(targetProperty)
{
    /// <inheritdoc />
    protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get; } = Expression.LessThan;
}