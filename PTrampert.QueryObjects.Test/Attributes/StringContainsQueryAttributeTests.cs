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
}

