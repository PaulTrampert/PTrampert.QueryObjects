using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// Entity Framework Core backed by a SQL Server container.
/// </summary>
public class SqlServerProductStore : EfCoreProductStore
{
    private readonly MsSqlContainer _container = new MsSqlBuilder(ContainerImages.SqlServer).Build();

    public override string StringLiteral(string value) => $"N'{value}'";

    protected override Task StartContainerAsync() => _container.StartAsync();

    protected override Task StopContainerAsync() => _container.DisposeAsync().AsTask();

    protected override void ConfigureProvider(DbContextOptionsBuilder<ProductDbContext> options)
    {
        // The container hands out a connection string pointed at master; give the tests their own database.
        var connectionString = new SqlConnectionStringBuilder(_container.GetConnectionString())
        {
            InitialCatalog = "QueryObjects"
        }.ConnectionString;

        options.UseSqlServer(connectionString);
    }
}
