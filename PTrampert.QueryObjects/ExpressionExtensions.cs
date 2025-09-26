using System;
using System.Linq.Expressions;
using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects;

/// <summary>
/// Extensions to allow easier combining of predicate expressions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Combines two lambda expressions with a logical AND.
    /// </summary>
    /// <param name="left">The base "this" lambda</param>
    /// <param name="right">The other lambda to be logically AND'ed with left.</param>
    /// <typeparam name="T">The parameter type.</typeparam>
    /// <returns>(T) => left.Body && right.Body</returns>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = left.Parameters[0];
        
        var parameterReplacer = new ParameterReplacementVisitor(right.Parameters[0], parameter);

        var newRight = parameterReplacer.Visit(right) as Expression<Func<T, bool>>;

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, newRight.Body), parameter);
    }
    
    /// <summary>
    /// Combines two lambda expressions with a logical OR.
    /// </summary>
    /// <param name="left">The base "this" lambda</param>
    /// <param name="right">The other lambda to be logically OR'ed with left.</param>
    /// <typeparam name="T">The parameter type.</typeparam>
    /// <returns>(T) => left.Body || right.Body</returns>
    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = left.Parameters[0];
        
        var parameterReplacer = new ParameterReplacementVisitor(right.Parameters[0], parameter);

        var newRight = parameterReplacer.Visit(right) as Expression<Func<T, bool>>;

        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, newRight.Body), parameter);
    }
}