using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class NotEqualsQueryAttributeTests
{
    private record TestTarget
    {
        public int Value { get; init; }
    }

    private record TestQuery
    {
        [NotEqualsQuery(nameof(TestTarget.Value))]
        public int? Value { get; init; }
    }

    [Test]
    public void NotEqualsQueryAttribute_SelectsRecordsWithNotEqualValue()
    {
        var data = new List<TestTarget>
        {
            new() { Value = 1 },
            new() { Value = 5 },
            new() { Value = 10 }
        };
        var query = new TestQuery { Value = 5 };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo([data[0], data[2]]));
    }
}

