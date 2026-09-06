namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// The container images the integration suites run against, pinned in one place so the versions under test are
/// explicit and easy to bump.
/// </summary>
public static class ContainerImages
{
    public const string SqlServer = "mcr.microsoft.com/mssql/server:2022-latest";

    public const string PostgreSql = "postgres:17-alpine";

    public const string MongoDb = "mongo:8.2";
}
