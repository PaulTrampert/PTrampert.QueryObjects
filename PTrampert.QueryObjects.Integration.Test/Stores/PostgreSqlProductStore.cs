using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// Entity Framework Core backed by a PostgreSQL container.
/// </summary>
public class PostgreSqlProductStore : EfCoreProductStore
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder(ContainerImages.PostgreSql).Build();

    public override string StringLiteral(string value) => $"'{value}'";

    protected override Task StartContainerAsync() => _container.StartAsync();

    protected override Task StopContainerAsync() => _container.DisposeAsync().AsTask();

    protected override void ConfigureProvider(DbContextOptionsBuilder<ProductDbContext> options)
    {
        var connectionString = new NpgsqlConnectionStringBuilder(_container.GetConnectionString())
        {
            Database = "queryobjects"
        }.ConnectionString;

        options.UseNpgsql(connectionString);
    }
}
