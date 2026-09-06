using Microsoft.EntityFrameworkCore;
using PTrampert.QueryObjects.Integration.Test.Model;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// Base class for the Entity Framework Core backed stores. Subclasses supply the container and the provider
/// specific configuration.
/// </summary>
public abstract class EfCoreProductStore : IProductStore
{
    private readonly CommandCapturingInterceptor _commandCapture = new();

    private ProductDbContext? _context;

    public IQueryable<Product> Products => Context.Products.AsNoTracking();

    /// <summary>
    /// The last command Entity Framework Core sent to the database.
    /// </summary>
    public CapturedCommand? LastCommand => _commandCapture.LastCommand;

    /// <summary>
    /// How the provider renders a string literal in SQL, used to assert whether a query value was inlined.
    /// </summary>
    public abstract string StringLiteral(string value);

    public async Task InitializeAsync(IReadOnlyList<Product> products)
    {
        await StartContainerAsync();

        var options = new DbContextOptionsBuilder<ProductDbContext>();
        ConfigureProvider(options);
        options.AddInterceptors(_commandCapture);

        _context = new ProductDbContext(options.Options);
        await _context.Database.EnsureCreatedAsync();
        // Seeded from copies so change tracking never mutates the shared test data.
        _context.Products.AddRange(products.Select(Copy));
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (_context != null)
        {
            await _context.DisposeAsync();
        }

        await StopContainerAsync();
    }

    protected abstract Task StartContainerAsync();

    protected abstract Task StopContainerAsync();

    protected abstract void ConfigureProvider(DbContextOptionsBuilder<ProductDbContext> options);

    private ProductDbContext Context =>
        _context ?? throw new InvalidOperationException("The store has not been initialized.");

    private static Product Copy(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Category = product.Category,
        Quantity = product.Quantity,
        Price = product.Price,
        ReleasedOn = product.ReleasedOn,
        Discontinued = product.Discontinued,
        Tags = [.. product.Tags]
    };
}
