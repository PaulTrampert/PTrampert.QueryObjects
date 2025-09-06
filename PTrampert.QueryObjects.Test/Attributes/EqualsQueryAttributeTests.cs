using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class EqualsQueryAttributeTests
{
    private record EqualsTestTarget
    {
        public Guid Id { get; init; }
    }
    
    private record EqualsTestQuery
    {
        [EqualsQuery(nameof(EqualsTestTarget.Id))]
        public Guid? Id { get; init; }
    }

    [Test]
    public void EqualsQueryAttribute_SelectsRecordsWithEqualValue()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        
        var data = new List<EqualsTestTarget>
        {
            new() { Id = id1 },
            new() { Id = id2 },
            new() { Id = id3 }
        };

        var queryObject = new EqualsTestQuery
        {
            Id = id2
        };

        // Act
        var result = data.Where(queryObject);
        
        Assert.That(result, Is.EqualTo([data[1]]));
    }
}