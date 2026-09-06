using PTrampert.QueryObjects.Integration.Test.Stores;

namespace PTrampert.QueryObjects.Integration.Test;

/// <summary>
/// Runs the shared suite against Entity Framework Core backed by SQL Server.
/// </summary>
[TestFixture]
[Category("SqlServer")]
public class SqlServerProductQueryTests : EfCoreProductQueryTests
{
    protected override EfCoreProductStore CreateEfStore() => new SqlServerProductStore();
}
