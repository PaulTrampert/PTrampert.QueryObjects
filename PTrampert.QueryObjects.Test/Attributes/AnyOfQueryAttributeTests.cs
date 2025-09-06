using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class AnyOfQueryAttributeTests
{
    private record AnyOfTestTarget
    {
        public Guid Id { get; init; }
    }
    
    private record AnyOfTestQuery
    {
        [AnyOfQuery(nameof(AnyOfTestTarget.Id))]
        public IEnumerable<Guid>? Ids { get; init; }
    }
    
    
    [Test]
    public void AnyOfAttribute_SelectsRecordsWithAnyOfTheSpecifiedValues()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        
        var data = new List<AnyOfTestTarget>
        {
            new() { Id = id1 },
            new() { Id = id2 },
            new() { Id = id3 }
        };

        var queryObject = new AnyOfTestQuery
        {
            Ids = new[] { id1, id3 }
        };

        // Act
        var result = data.Where(queryObject);
        
        Assert.That(result, Is.EqualTo([data[0], data[2]]));
    }
}