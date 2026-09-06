using PTrampert.QueryObjects.Integration.Test.Model;
using PTrampert.QueryObjects.Integration.Test.Stores;

namespace PTrampert.QueryObjects.Integration.Test;

/// <summary>
/// The shared integration suite. Every database under test derives from this class and supplies its own
/// <see cref="IProductStore"/>, so the same expectations run against every provider.
///
/// Each case builds a query object, hands it to <see cref="QueryableExtensions.Where{T,TQuery}(IQueryable{T},TQuery)"/>
/// and enumerates the result. Because all of these providers throw rather than silently fall back to client side
/// evaluation, a passing test also proves the predicate was translated and executed by the database.
/// </summary>
public abstract class ProductQueryTests
{
    /// <summary>
    /// The database under test, seeded with <see cref="TestProducts.All"/>.
    /// </summary>
    protected IProductStore Store { get; private set; } = null!;

    /// <summary>
    /// Creates the store under test. Called once per fixture.
    /// </summary>
    protected abstract IProductStore CreateStore();

    [OneTimeSetUp]
    public async Task StartDatabase()
    {
        Store = CreateStore();
        await Store.InitializeAsync(TestProducts.All);
    }

    [OneTimeTearDown]
    public async Task StopDatabase()
    {
        if (Store != null)
        {
            await Store.DisposeAsync();
        }
    }

    [Test]
    public void SeedData_RoundTrips()
    {
        var carrot = Store.Products.Where(new ProductQuery { Id = TestProducts.Carrot.Id }).Single();

        Assert.Multiple(() =>
        {
            Assert.That(carrot.Name, Is.EqualTo(TestProducts.Carrot.Name));
            Assert.That(carrot.Category, Is.EqualTo(TestProducts.Carrot.Category));
            Assert.That(carrot.Quantity, Is.EqualTo(TestProducts.Carrot.Quantity));
            Assert.That(carrot.Price, Is.EqualTo(TestProducts.Carrot.Price));
            Assert.That(carrot.ReleasedOn, Is.EqualTo(TestProducts.Carrot.ReleasedOn).Within(TimeSpan.Zero));
            Assert.That(carrot.Discontinued, Is.EqualTo(TestProducts.Carrot.Discontinued));
            Assert.That(carrot.Tags, Is.EqualTo(TestProducts.Carrot.Tags));
        });
    }

    [Test]
    public void EmptyQuery_MatchesEverything()
    {
        AssertMatches(new ProductQuery(), TestProducts.All.ToArray());
    }

    [Test]
    public void EqualsQuery_MatchesOnAString()
    {
        AssertMatches(
            new ProductQuery { Category = "Fruit" },
            TestProducts.Apple, TestProducts.Apricot, TestProducts.Banana);
    }

    [Test]
    public void EqualsQuery_MatchesOnAGuid()
    {
        AssertMatches(new ProductQuery { Id = TestProducts.Cashew.Id }, TestProducts.Cashew);
    }

    [Test]
    public void EqualsQuery_MatchesOnABoolean()
    {
        AssertMatches(
            new ProductQuery { Discontinued = true },
            TestProducts.Banana, TestProducts.Chestnut);
    }

    [Test]
    public void NotEqualsQuery_ExcludesMatches()
    {
        AssertMatches(
            new ProductQuery { CategoryNot = "Fruit" },
            TestProducts.Broccoli, TestProducts.Carrot, TestProducts.Cashew, TestProducts.Chestnut);
    }

    [Test]
    public void GreaterThanAndLessThanQueries_BoundAnExclusiveRange()
    {
        AssertMatches(
            new ProductQuery { QuantityGreaterThan = 2, QuantityLessThan = 20 },
            TestProducts.Apple, TestProducts.Apricot, TestProducts.Carrot);
    }

    [Test]
    public void GreaterThanOrEqualAndLessThanOrEqualQueries_BoundAnInclusiveRange()
    {
        AssertMatches(
            new ProductQuery { QuantityAtLeast = 2, QuantityAtMost = 20 },
            TestProducts.Apple, TestProducts.Apricot, TestProducts.Broccoli, TestProducts.Carrot,
            TestProducts.Chestnut);
    }

    [Test]
    public void ComparisonQueries_CompareDecimalsNumerically()
    {
        AssertMatches(
            new ProductQuery { PriceAtLeast = 3.00m },
            TestProducts.Broccoli, TestProducts.Cashew, TestProducts.Chestnut);
    }

    [Test]
    public void ComparisonQueries_CompareDatesChronologically()
    {
        AssertMatches(
            new ProductQuery
            {
                ReleasedOnOrAfter = TestProducts.Carrot.ReleasedOn,
                ReleasedBefore = TestProducts.Chestnut.ReleasedOn
            },
            TestProducts.Carrot, TestProducts.Cashew);
    }

    [Test]
    public void StringContainsQuery_MatchesASubstring()
    {
        AssertMatches(new ProductQuery { NameContains = "an" }, TestProducts.Banana);
    }

    [Test]
    public void StringStartsWithQuery_MatchesAPrefix()
    {
        AssertMatches(
            new ProductQuery { NameStartsWith = "C" },
            TestProducts.Carrot, TestProducts.Cashew, TestProducts.Chestnut);
    }

    [Test]
    public void AnyOfQuery_MatchesAScalarAgainstACollectionOfValues()
    {
        AssertMatches(
            new ProductQuery { Categories = ["Fruit", "Nut"] },
            TestProducts.Apple, TestProducts.Apricot, TestProducts.Banana, TestProducts.Cashew,
            TestProducts.Chestnut);
    }

    [Test]
    public void NoneOfQuery_ExcludesAScalarFoundInACollectionOfValues()
    {
        AssertMatches(
            new ProductQuery { ExcludedCategories = ["Fruit", "Nut"] },
            TestProducts.Broccoli, TestProducts.Carrot);
    }

    [Test]
    public void ContainsQuery_MatchesAValueHeldByACollectionProperty()
    {
        AssertMatches(new ProductQuery { Tag = "orange" }, TestProducts.Apricot, TestProducts.Carrot);
    }

    [Test]
    public void AnyOfQuery_MatchesWhenACollectionPropertyOverlapsTheQueryValues()
    {
        AssertMatches(
            new ProductQuery { AnyTags = ["root", "roasted"] },
            TestProducts.Carrot, TestProducts.Chestnut);
    }

    [Test]
    public void NoneOfQuery_ExcludesWhenACollectionPropertyOverlapsTheQueryValues()
    {
        AssertMatches(
            new ProductQuery { NoneOfTags = ["fresh"] },
            TestProducts.Cashew, TestProducts.Chestnut);
    }

    [Test]
    public void MultipleCriteria_AreCombinedWithAnd()
    {
        AssertMatches(
            new ProductQuery
            {
                Categories = ["Fruit", "Vegetable"],
                Tag = "fresh",
                QuantityGreaterThan = 0,
                PriceLessThan = 2.00m,
                NameStartsWith = "A"
            },
            TestProducts.Apple);
    }

    [Test]
    public void NoMatches_ReturnsAnEmptyResult()
    {
        AssertMatches(new ProductQuery { Category = "Mineral" });
    }

    [Test]
    public void QueryObjectExpression_IsCombinedWithTheAttributeFilters()
    {
        AssertMatches(
            new InStockProductQuery { Category = "Fruit" },
            TestProducts.Apple, TestProducts.Apricot);
    }

    /// <summary>
    /// Filters the seeded data with <paramref name="query"/> and asserts the database returned exactly
    /// <paramref name="expected"/>, in any order.
    /// </summary>
    protected void AssertMatches<TQuery>(TQuery query, params Product[] expected)
    {
        var matches = Store.Products.Where(query).ToList();

        Assert.That(
            matches.Select(p => p.Name),
            Is.EquivalentTo(expected.Select(p => p.Name)));
    }
}
