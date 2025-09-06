using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class ContainsQueryAttributeTests
{
    private record TestTarget
    {
        public IEnumerable<string> Values { get; init; } = null!;
    }
    
    private record TestQuery
    {
        [ContainsQuery(nameof(TestTarget.Values))]
        public string? Value { get; init; }
    }

    [Test]
    public void ContainsQueryAttribute_SelectsRecordsWithContainedValue()
    {
        var data = new List<TestTarget>
        {
            new() { Values = ["apple", "banana"] },
            new() { Values = ["banana", "grape"] },
            new() { Values = ["pineapple", "kiwi"] }
        };
        var query = new TestQuery { Value = "banana" };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo([data[0], data[1]]));
    }
}