using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class StringStartsWithQueryAttributeTests
{
    private record TestTarget
    {
        public string? Name { get; init; }
    }

    private record TestQuery
    {
        [StringStartsWithQuery(nameof(TestTarget.Name), IgnoreIfNull = false)]
        public string? Name { get; init; }
    }
    
    private record TestQueryIgnorNull
    {
        [StringStartsWithQuery(nameof(TestTarget.Name))]
        public string? Name { get; init; }
    }

    [Test]
    public void StringStartsWithQueryAttribute_SelectsRecordsStartingWithSubstring()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "apricot" },
            new() { Name = "banana" },
            new() { Name = "application" },
            new() { Name = "grape" },
            new() { Name = "trapped"}
        };
        var query = new TestQuery { Name = "app" };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo([data[0], data[3]]));
    }
    
    [Test]
    public void StringStartsWithQueryAttribute_WhenIgnoreIfNullTrue_NullQueryValue_ReturnsAllRecords()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "banana" },
            new() { Name = "grape" }
        };
        var query = new TestQueryIgnorNull { Name = null };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo(data));
    }
    
    [Test]
    public void StringStartsWithQueryAttribute_WhenIgnoreIfNullFalse_NullQueryValue_ReturnsNoRecords()
    {
        var data = new List<TestTarget>
        {
            new() { Name = "apple" },
            new() { Name = "banana" },
            new() { Name = "grape" }
        };
        var query = new TestQuery { Name = null };
        var result = data.Where(query);
        Assert.That(result, Is.Empty);
    }
}