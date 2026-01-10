using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Checks if the target string property starts with the query string property.
///
/// Equivalent linq expression: <code>queryable.Where(x => x.TargetProperty.StartsWith(queryObject.QueryProperty))</code>
/// </summary>
public class StringStartsWithQueryAttribute : QueryAttribute
{

    /// <inheritdoc />
    public StringStartsWithQueryAttribute(string targetProperty = null) : base(targetProperty)
    {
    }

    /// <summary>
    /// Ignore the search term if it is null.
    /// </summary>
    public bool IgnoreIfNull { get; set; } = true;
    
    /// <inheritdoc />
    public override Expression BuildExpression(
        object queryObject, 
        PropertyInfo queryProperty, 
        ParameterExpression targetParameter,
        PropertyInfo targetProperty
    )
    {
        if (queryObject == null) throw new ArgumentNullException(nameof(queryObject));
        if (queryProperty == null) throw new ArgumentNullException(nameof(queryProperty));
        if (targetParameter == null) throw new ArgumentNullException(nameof(targetParameter));
        if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));
        
        var queryValue = queryProperty.GetValue(queryObject);
        if (queryValue == null)
            return IgnoreIfNull ? null : Expression.Constant(false);
        
        var constant = Expression.Constant(queryValue);
        var startsWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!;
        return Expression.Call(Expression.Property(targetParameter, targetProperty), startsWithMethod, constant);
    }
}