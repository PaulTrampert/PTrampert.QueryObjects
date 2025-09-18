using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class StringContainsQueryAttributeTests
{
    private record TestTarget
    {
        public string? Name { get; init; }
    }

    private record TestQuery
    {
        [StringContainsQuery(nameof(TestTarget.Name), IgnoreIfNull = false)]
        public string? Name { get; init; }
    }

    private record IgnoreIfNullTestQuery
    {
        [StringContainsQuery(nameof(TestTarget.Name))]
        public string? Name { get; init; }
    }

    [Test]
    public void StringContainsQueryAttribute_SelectsRecordsContainingSubstring()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "banana" },
            new() { Name = "pineapple" },
            new() { Name = "grape" }
        };
        var query = new TestQuery { Name = "apple" };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo([data[0], data[2]]));
    }

    [Test]
    public void StringContainsQueryAttribute_IgnoreIfNull_True_DoesNotFilterIfNull()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "banana" },
            new() { Name = "pineapple" },
            new() { Name = "grape" }
        };
        var query = new IgnoreIfNullTestQuery { Name = null };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo(data));
    }

    [Test]
    public void StringContainsQueryAttribute_IgnoreIfNull_True_FiltersIfNotNull()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "banana" },
            new() { Name = "pineapple" },
            new() { Name = "grape" }
        };
        var query = new IgnoreIfNullTestQuery { Name = "app" };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo([data[0], data[2]]));
    }

    [Test]
    public void StringContainsQueryAttribute_IgnoreIfNull_False_FiltersAllIfNull()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "banana" },
            new() { Name = "pineapple" },
            new() { Name = "grape" }
        };
        var query = new TestQuery { Name = null };
        var result = data.Where(query);
        Assert.That(result, Is.Empty);
    }
}
