using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test.Attributes;

public class NoneOfQueryTests
{
    private record NoneOfTestTarget
    {
        public Guid Id { get; init; }
    }
    
    private record NoneOfTestTargetWithEnumerable
    {
        public IEnumerable<Guid> Ids { get; init; } = null!;
    }
    
    private record NoneOfTestQuery
    {
        [NoneOfQuery(nameof(NoneOfTestTarget.Id))]
        public IEnumerable<Guid>? Ids { get; init; }
    }
    
    private record NoneOfTestQueryWithEnumerable
    {
        [NoneOfQuery]
        public IEnumerable<Guid>? Ids { get; init; }
    }

    [Test]
    public void NoneOfQueryAttribute_ExcludesRecordsWithAnyOfTheSpecifiedValues()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        
        var data = new List<NoneOfTestTarget>
        {
            new() { Id = id1 },
            new() { Id = id2 },
            new() { Id = id3 }
        };

        var queryObject = new NoneOfTestQuery
        {
            Ids = new[] { id1, id3 }
        };

        // Act
        var result = data.Where(queryObject);
        
        Assert.That(result, Is.EqualTo([data[1]]));
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
        
        var data = new List<NoneOfTestTargetWithEnumerable>
        {
            new() { Ids = [id1, id2] },
            new() { Ids = [id3, id4] },
            new() { Ids = [id5, id6] }
        };
        var queryObject = new NoneOfTestQueryWithEnumerable
        {
            Ids = [id2, id5]
        };
        
        var result = data.Where(queryObject);

        Assert.That(result, Is.EqualTo([data[1]]));
    }
}