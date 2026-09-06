using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PTrampert.QueryObjects.Integration.Test.Model;
using Testcontainers.MongoDb;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// The MongoDB C# driver's LINQ provider backed by a MongoDB container.
/// </summary>
public class MongoDbProductStore : IProductStore
{
    private static readonly object SerializerLock = new();
    private static bool _serializersRegistered;

    private readonly MongoDbContainer _container = new MongoDbBuilder(ContainerImages.MongoDb).Build();

    private IMongoCollection<Product>? _collection;

    public IQueryable<Product> Products =>
        (_collection ?? throw new InvalidOperationException("The store has not been initialized.")).AsQueryable();

    public async Task InitializeAsync(IReadOnlyList<Product> products)
    {
        RegisterSerializers();
        await _container.StartAsync();
        var client = new MongoClient(_container.GetConnectionString());
        _collection = client.GetDatabase("queryobjects").GetCollection<Product>("products");
        await _collection.InsertManyAsync(products);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _container.DisposeAsync();
    }

    /// <summary>
    /// The driver has no default representation for <see cref="Guid"/>, and represents <see cref="decimal"/> as a
    /// string, which would order comparisons lexically. Both are pinned to their BSON equivalents so the database
    /// compares them the same way the relational providers do.
    /// </summary>
    private static void RegisterSerializers()
    {
        lock (SerializerLock)
        {
            if (_serializersRegistered) return;
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            _serializersRegistered = true;
        }
    }
}
