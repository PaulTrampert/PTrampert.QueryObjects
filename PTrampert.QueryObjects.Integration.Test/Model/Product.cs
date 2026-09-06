namespace PTrampert.QueryObjects.Integration.Test.Model;

/// <summary>
/// The document/row shape all of the integration suites filter against. Deliberately provider agnostic:
/// each store is responsible for mapping it onto its own database.
/// </summary>
public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime ReleasedOn { get; set; }

    public bool Discontinued { get; set; }

    public List<string> Tags { get; set; } = new();
}
