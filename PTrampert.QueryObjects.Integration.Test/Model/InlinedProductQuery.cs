using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Integration.Test.Model;

/// <summary>
/// The same filter as <see cref="ProductQuery"/>'s category equality, but opting into
/// <see cref="QueryAttribute.InlineValue"/> so the value is baked into the query instead of parameterized.
/// </summary>
public class InlinedProductQuery
{
    [EqualsQuery(nameof(Product.Category), InlineValue = true)]
    public string? Category { get; set; }
}
