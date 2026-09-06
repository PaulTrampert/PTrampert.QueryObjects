namespace PTrampert.QueryObjects.Integration.Test.Model;

/// <summary>
/// The fixed data set every integration suite is seeded with. Names are unique and are used as the identity in
/// assertions, so that round trip fidelity of the other columns never affects the outcome of a filter test.
/// </summary>
public static class TestProducts
{
    public static readonly Product Apple = new()
    {
        Id = Id(1),
        Name = "Apple",
        Category = "Fruit",
        Quantity = 10,
        Price = 1.50m,
        ReleasedOn = Released(2024, 1, 1),
        Discontinued = false,
        Tags = ["fresh", "red"]
    };

    public static readonly Product Apricot = new()
    {
        Id = Id(2),
        Name = "Apricot",
        Category = "Fruit",
        Quantity = 5,
        Price = 2.25m,
        ReleasedOn = Released(2024, 2, 15),
        Discontinued = false,
        Tags = ["fresh", "orange"]
    };

    public static readonly Product Banana = new()
    {
        Id = Id(3),
        Name = "Banana",
        Category = "Fruit",
        Quantity = 0,
        Price = 0.75m,
        ReleasedOn = Released(2024, 3, 10),
        Discontinued = true,
        Tags = ["fresh", "yellow"]
    };

    public static readonly Product Broccoli = new()
    {
        Id = Id(4),
        Name = "Broccoli",
        Category = "Vegetable",
        Quantity = 20,
        Price = 3.00m,
        ReleasedOn = Released(2024, 4, 20),
        Discontinued = false,
        Tags = ["fresh", "green"]
    };

    public static readonly Product Carrot = new()
    {
        Id = Id(5),
        Name = "Carrot",
        Category = "Vegetable",
        Quantity = 15,
        Price = 1.25m,
        ReleasedOn = Released(2024, 5, 5),
        Discontinued = false,
        Tags = ["fresh", "orange", "root"]
    };

    public static readonly Product Cashew = new()
    {
        Id = Id(6),
        Name = "Cashew",
        Category = "Nut",
        Quantity = 30,
        Price = 8.99m,
        ReleasedOn = Released(2024, 6, 30),
        Discontinued = false,
        Tags = ["dry", "snack"]
    };

    public static readonly Product Chestnut = new()
    {
        Id = Id(7),
        Name = "Chestnut",
        Category = "Nut",
        Quantity = 2,
        Price = 12.50m,
        ReleasedOn = Released(2024, 7, 4),
        Discontinued = true,
        Tags = ["dry", "roasted"]
    };

    public static IReadOnlyList<Product> All { get; } =
        [Apple, Apricot, Banana, Broccoli, Carrot, Cashew, Chestnut];

    /// <summary>
    /// Dates are whole days in UTC so that every provider's date/time precision and time zone handling round trips
    /// them identically.
    /// </summary>
    private static DateTime Released(int year, int month, int day) =>
        new(year, month, day, 0, 0, 0, DateTimeKind.Utc);

    private static Guid Id(int seed) => new($"00000000-0000-0000-0000-{seed:D12}");
}
