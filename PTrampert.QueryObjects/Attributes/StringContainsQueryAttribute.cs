using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Perform a Contains query on the applied property.
/// </summary>
public class StringContainsQueryAttribute : QueryAttribute
{
    /// <summary>
    /// Ignore the search term if not included.
    /// </summary>
    public bool IgnoreIfNull { get; init; }

    /// <inheritdoc />
    public override Expression? BuildExpression(object queryObject, PropertyInfo queryProperty, ParameterExpression targetParameter,
        PropertyInfo? targetProperty)
    {
        ArgumentNullException.ThrowIfNull(queryObject, nameof(queryObject));
        ArgumentNullException.ThrowIfNull(queryProperty, nameof(queryProperty));
        ArgumentNullException.ThrowIfNull(targetParameter, nameof(targetParameter));
        ArgumentNullException.ThrowIfNull(targetProperty, nameof(targetProperty));

        var queryValue = queryProperty.GetValue(queryObject);
        if (IgnoreIfNull && queryValue == null)
            return null;
        var constant = Expression.Constant(queryValue);
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)});
        return Expression.Call(Expression.Property(targetParameter, targetProperty), containsMethod!, constant);
    }
}