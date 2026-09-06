using System.Linq.Expressions;
using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Integration.Test.Model;

/// <summary>
/// A query object that mixes attribute driven filtering with a hand written expression supplied through
/// <see cref="IQueryObject{T}"/>, to prove the two are combined into a single translatable predicate.
/// </summary>
public class InStockProductQuery : IQueryObject<Product>
{
    [EqualsQuery(nameof(Product.Category))]
    public string? Category { get; set; }

    public Expression<Func<Product, bool>> BuildQueryExpression() => p => p.Quantity > 0 && !p.Discontinued;
}
