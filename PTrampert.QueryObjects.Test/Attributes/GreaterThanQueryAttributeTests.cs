using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class GreaterThanQueryAttributeTests
{
    private record TestTarget
    {
        public int Value { get; init; }
    }

    private record TestQuery
    {
        [GreaterThanQuery(nameof(TestTarget.Value))]
        public int? Value { get; init; }
    }

    [Test]
    public void GreaterThanQueryAttribute_SelectsRecordsWithGreaterValue()
    {
        var data = new List<TestTarget>
        {
            new() { Value = 1 },
            new() { Value = 5 },
            new() { Value = 10 }
        };
        var query = new TestQuery { Value = 4 };
        var result = data.Where(query);
        Assert.That(result, Is.EqualTo([data[1], data[2]]));
    }
}

