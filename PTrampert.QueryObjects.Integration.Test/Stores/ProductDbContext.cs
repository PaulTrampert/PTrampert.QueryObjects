using Microsoft.EntityFrameworkCore;
using PTrampert.QueryObjects.Integration.Test.Model;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// The Entity Framework Core model shared by the SQL Server and PostgreSQL suites.
/// </summary>
public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();
        product.HasKey(p => p.Id);
        product.Property(p => p.Name).HasMaxLength(100).IsRequired();
        product.Property(p => p.Category).HasMaxLength(100).IsRequired();
        product.Property(p => p.Price).HasPrecision(18, 2);
        // Mapped as a primitive collection, which both providers store as JSON and can query in the database.
        product.PrimitiveCollection(p => p.Tags).IsRequired();
    }
}
