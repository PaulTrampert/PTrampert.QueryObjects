using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Integration.Test.Model;

/// <summary>
/// A single query object exercising every <see cref="QueryAttribute"/> the library ships with. Every property is
/// nullable so that an unset property is skipped, which lets one query object drive every test case.
/// </summary>
public class ProductQuery
{
    [EqualsQuery(nameof(Product.Id))]
    public Guid? Id { get; set; }

    [EqualsQuery(nameof(Product.Category))]
    public string? Category { get; set; }

    [NotEqualsQuery(nameof(Product.Category))]
    public string? CategoryNot { get; set; }

    [EqualsQuery(nameof(Product.Discontinued))]
    public bool? Discontinued { get; set; }

    [GreaterThanQuery(nameof(Product.Quantity))]
    public int? QuantityGreaterThan { get; set; }

    [GreaterThanOrEqualQuery(nameof(Product.Quantity))]
    public int? QuantityAtLeast { get; set; }

    [LessThanQuery(nameof(Product.Quantity))]
    public int? QuantityLessThan { get; set; }

    [LessThanOrEqualQuery(nameof(Product.Quantity))]
    public int? QuantityAtMost { get; set; }

    [GreaterThanOrEqualQuery(nameof(Product.Price))]
    public decimal? PriceAtLeast { get; set; }

    [LessThanQuery(nameof(Product.Price))]
    public decimal? PriceLessThan { get; set; }

    [GreaterThanOrEqualQuery(nameof(Product.ReleasedOn))]
    public DateTime? ReleasedOnOrAfter { get; set; }

    [LessThanQuery(nameof(Product.ReleasedOn))]
    public DateTime? ReleasedBefore { get; set; }

    [StringContainsQuery(nameof(Product.Name))]
    public string? NameContains { get; set; }

    [StringStartsWithQuery(nameof(Product.Name))]
    public string? NameStartsWith { get; set; }

    [AnyOfQuery(nameof(Product.Category))]
    public IEnumerable<string>? Categories { get; set; }

    [NoneOfQuery(nameof(Product.Category))]
    public IEnumerable<string>? ExcludedCategories { get; set; }

    [ContainsQuery(nameof(Product.Tags))]
    public string? Tag { get; set; }

    [AnyOfQuery(nameof(Product.Tags))]
    public IEnumerable<string>? AnyTags { get; set; }

    [NoneOfQuery(nameof(Product.Tags))]
    public IEnumerable<string>? NoneOfTags { get; set; }
}
