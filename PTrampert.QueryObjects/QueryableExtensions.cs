

using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects;

public static class QueryableExtensions
{
    /// <summary>
    /// Filters a queryable based on the properties of a query object. By default, properties are matched by name, and
    /// only properties annotated with <see cref="Attributes.QueryAttribute"/> are included in the filter. This behavior
    /// can be extended by implementing <see cref="IQueryObject{T}"/> on the query object.
    /// </summary>
    /// <param name="queryable">The queryable to be fitered.</param>
    /// <param name="queryObject">The query object used to build the filter.</param>
    /// <typeparam name="T">The type being filtered.</typeparam>
    /// <returns>The filtered <see cref="IQueryable{T}"/></returns>
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, object queryObject)
    {
        var expressionBuilder = new QueryExpressionBuilder<T>();
        var expression = expressionBuilder.BuildQueryExpression(queryObject);
        return Queryable.Where(queryable, expression);
    }
    
    public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, object queryObject)
    {
        return enumerable.AsQueryable().Where(queryObject);
    }
}