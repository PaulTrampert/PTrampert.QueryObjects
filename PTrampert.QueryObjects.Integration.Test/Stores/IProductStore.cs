using PTrampert.QueryObjects.Integration.Test.Model;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// A database under test. Implementations own the container, the schema and the seeding, and expose the seeded
/// data as an <see cref="IQueryable{T}"/> that the shared test suite filters through query objects.
/// </summary>
public interface IProductStore : IAsyncDisposable
{
    /// <summary>
    /// Starts the database container, creates the schema and inserts <paramref name="products"/>.
    /// </summary>
    Task InitializeAsync(IReadOnlyList<Product> products);

    /// <summary>
    /// A queryable rooted at the database. Enumerating it must send the filter to the database rather than
    /// evaluating it client side.
    /// </summary>
    IQueryable<Product> Products { get; }
}
