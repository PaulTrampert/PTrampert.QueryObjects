using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Attributes;

/// <summary>
/// Interface that defines a query object. Query objects are used to build expressions for filtering data.
/// This interface is only necessary if you need to build a query expression that cannot be expressed using
/// <see cref="QueryAttribute"/>s.
/// </summary>
/// <typeparam name="T">The database type this query object filters.</typeparam>
public interface IQueryObject<T>
{
    /// <summary>
    /// Builds a query expression for filtering data.
    /// </summary>
    /// <returns>A query expression.</returns>
    Expression<Func<T, bool>>? BuildQueryExpression();
}