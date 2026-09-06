using PTrampert.QueryObjects.Integration.Test.Stores;

namespace PTrampert.QueryObjects.Integration.Test;

/// <summary>
/// Runs the shared suite against Entity Framework Core backed by PostgreSQL.
/// </summary>
[TestFixture]
[Category("PostgreSql")]
public class PostgreSqlProductQueryTests : EfCoreProductQueryTests
{
    protected override EfCoreProductStore CreateEfStore() => new PostgreSqlProductStore();
}
