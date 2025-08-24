using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Checks if the target property does not equal the query property.
/// </summary>
public class NotEqualsQueryAttribute(string? targetProperty = null) : SimpleComparisonQueryAttribute(targetProperty)
{
    /// <inheritdoc />
    protected override Func<Expression, Expression, Expression> ComparisonExpressionBuilder { get; } = Expression.NotEqual;
}