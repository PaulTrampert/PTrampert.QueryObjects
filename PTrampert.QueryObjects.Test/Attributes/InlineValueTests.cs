using PTrampert.QueryObjects.Attributes;
using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects.Test.Attributes;

public class InlineValueTests
{
    private class InlineTestTarget
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public List<string> Tags { get; init; } = new();
    }

    private class ParameterizedQuery
    {
        [EqualsQuery(nameof(InlineTestTarget.Id))]
        public Guid? Id { get; set; }

        [StringContainsQuery(nameof(InlineTestTarget.Name))]
        public string? NameContains { get; set; }

        [StringStartsWithQuery(nameof(InlineTestTarget.Name))]
        public string? NameStartsWith { get; set; }

        [ContainsQuery(nameof(InlineTestTarget.Tags))]
        public string? Tag { get; set; }
    }

    private class AnyOfParameterizedQuery
    {
        [AnyOfQuery(nameof(InlineTestTarget.Name))]
        public IEnumerable<string>? Names { get; set; }
    }

    private class AnyOfCollectionParameterizedQuery
    {
        [AnyOfQuery(nameof(InlineTestTarget.Tags))]
        public IEnumerable<string>? Tags { get; set; }
    }

    private class InlinedQuery
    {
        [EqualsQuery(nameof(InlineTestTarget.Id), InlineValue = true)]
        public Guid? Id { get; set; }

        [StringContainsQuery(nameof(InlineTestTarget.Name), InlineValue = true)]
        public string? NameContains { get; set; }
    }

    private static string BuildExpression(object query) =>
        new QueryExpressionBuilder<InlineTestTarget>().BuildQueryExpression(query).ToString();

    [Test]
    public void ByDefault_QueryValuesAreReferencedThroughTheQueryObject()
    {
        var query = new ParameterizedQuery
        {
            Id = Guid.NewGuid(),
            NameContains = "app",
            NameStartsWith = "a",
            Tag = "fruit"
        };

        var expression = BuildExpression(query);

        const string q = "value(PTrampert.QueryObjects.Test.Attributes.InlineValueTests+ParameterizedQuery)";
        Assert.Multiple(() =>
        {
            // The nullable query property is converted to the target property's type.
            Assert.That(expression, Does.Contain($"Convert({q}.Id, Guid)"));
            Assert.That(expression, Does.Contain($"Contains({q}.NameContains)"));
            Assert.That(expression, Does.Contain($"StartsWith({q}.NameStartsWith)"));
            Assert.That(expression, Does.Contain($"Param_0.Tags.Contains({q}.Tag)"));
            Assert.That(expression, Does.Not.Contain("\"app\""));
            Assert.That(expression, Does.Not.Contain(query.Id.ToString()));
        });
    }

    [Test]
    public void ByDefault_AnyOfReferencesTheCollectionThroughTheQueryObject()
    {
        var query = new AnyOfParameterizedQuery { Names = ["apple", "banana"] };

        var expression = BuildExpression(query);

        Assert.That(expression, Does.Contain(
            "value(PTrampert.QueryObjects.Test.Attributes.InlineValueTests+AnyOfParameterizedQuery).Names.Contains(Param_0.Name)"));
    }

    [Test]
    public void ByDefault_AnyOfAgainstACollectionTargetReferencesTheCollectionThroughTheQueryObject()
    {
        var query = new AnyOfCollectionParameterizedQuery { Tags = ["fruit", "yellow"] };

        var expression = BuildExpression(query);

        Assert.That(expression, Does.Contain(
            "Param_0.Tags.Intersect(value(PTrampert.QueryObjects.Test.Attributes.InlineValueTests+AnyOfCollectionParameterizedQuery).Tags).Any()"));
    }

    [Test]
    public void WithInlineValue_QueryValuesAreEmbeddedAsConstants()
    {
        var id = Guid.NewGuid();
        var query = new InlinedQuery { Id = id, NameContains = "app" };

        var expression = BuildExpression(query);

        Assert.Multiple(() =>
        {
            Assert.That(expression, Does.Contain($"Param_0.Id == {id}"));
            Assert.That(expression, Does.Contain("Contains(\"app\")"));
            Assert.That(expression, Does.Not.Contain("value(PTrampert.QueryObjects.Test.Attributes.InlineValueTests+InlinedQuery)"));
        });
    }

    [Test]
    public void BothValueStrategies_ProduceTheSameResults()
    {
        var id = Guid.NewGuid();
        var data = new List<InlineTestTarget>
        {
            new() { Id = id, Name = "apple", Tags = ["fruit"] },
            new() { Id = Guid.NewGuid(), Name = "banana", Tags = ["fruit"] }
        };

        var parameterized = data.Where(new ParameterizedQuery { Id = id, NameContains = "app" }).ToList();
        var inlined = data.Where(new InlinedQuery { Id = id, NameContains = "app" }).ToList();

        Assert.That(parameterized, Is.EqualTo(new[] { data[0] }));
        Assert.That(inlined, Is.EqualTo(parameterized));
    }
}
