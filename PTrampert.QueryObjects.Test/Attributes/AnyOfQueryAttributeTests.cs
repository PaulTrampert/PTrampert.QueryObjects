using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class AnyOfQueryAttributeTests
{
    private record AnyOfTestTarget
    {
        public Guid Id { get; init; }
    }

    private record AnyOfTestTargetWithEnumerable
    {
        public IEnumerable<Guid> Ids { get; init; } = null!;
    }
    
    private record AnyOfTestQuery
    {
        [AnyOfQuery(nameof(AnyOfTestTarget.Id))]
        public IEnumerable<Guid>? Ids { get; init; }
    }
    
    private record AnyOfTestQueryWithEnumerable
    {
        [AnyOfQuery]
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

    [Test]
    public void AnyOfAttribute_SelectsRecordsWithAnyOfTheSpecifiedValues_InEnumerableProperty()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        var id4 = Guid.NewGuid();
        var id5 = Guid.NewGuid();
        var id6 = Guid.NewGuid();
        
        var data = new List<AnyOfTestTargetWithEnumerable>
        {
            new() { Ids = [id1, id2] },
            new() { Ids = [id3, id4] },
            new() { Ids = [id5, id6] }
        };
        var queryObject = new AnyOfTestQueryWithEnumerable
        {
            Ids = [id2, id5]
        };
        
        var result = data.Where(queryObject);

        Assert.That(result, Is.EqualTo([data[0], data[2]]));
    }
}