using PTrampert.QueryObjects.Integration.Test.Stores;

namespace PTrampert.QueryObjects.Integration.Test;

/// <summary>
/// Runs the shared suite against the MongoDB C# driver's LINQ provider.
/// </summary>
[TestFixture]
[Category("MongoDb")]
public class MongoDbProductQueryTests : ProductQueryTests
{
    protected override IProductStore CreateStore() => new MongoDbProductStore();
}
