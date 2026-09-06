using PTrampert.QueryObjects.Integration.Test.Model;
using PTrampert.QueryObjects.Integration.Test.Stores;

namespace PTrampert.QueryObjects.Integration.Test;

/// <summary>
/// The shared suite plus the assertions that only make sense for a relational provider: that query values reach
/// the database as SQL parameters, so Entity Framework Core's compiled query cache and the database's plan cache
/// both stay effective, and that <see cref="Attributes.QueryAttribute.InlineValue"/> opts out of that.
/// </summary>
public abstract class EfCoreProductQueryTests : ProductQueryTests
{
    private EfCoreProductStore EfStore => (EfCoreProductStore)Store;

    protected sealed override IProductStore CreateStore() => CreateEfStore();

    protected abstract EfCoreProductStore CreateEfStore();

    [Test]
    public void QueryValues_AreSentAsParametersRatherThanSqlLiterals()
    {
        AssertMatches(
            new ProductQuery { Category = "Fruit", NameStartsWith = "Ap" },
            TestProducts.Apple, TestProducts.Apricot);

        var command = LastCommand();

        Assert.Multiple(() =>
        {
            Assert.That(command.Sql, Does.Not.Contain(EfStore.StringLiteral("Fruit")));
            Assert.That(command.Sql, Does.Not.Contain(EfStore.StringLiteral("Ap")));
            Assert.That(command.Parameters.Values, Does.Contain("Fruit"));
        });
    }

    [Test]
    public void TheSameQueryShape_ProducesTheSameSqlForDifferentValues()
    {
        AssertMatches(new ProductQuery { Category = "Fruit" }, TestProducts.Apple, TestProducts.Apricot, TestProducts.Banana);
        var fruitSql = LastCommand().Sql;

        AssertMatches(new ProductQuery { Category = "Nut" }, TestProducts.Cashew, TestProducts.Chestnut);
        var nutSql = LastCommand().Sql;

        Assert.That(nutSql, Is.EqualTo(fruitSql));
    }

    [Test]
    public void InlineValue_EmbedsTheQueryValueInTheSql()
    {
        AssertMatches(
            new InlinedProductQuery { Category = "Fruit" },
            TestProducts.Apple, TestProducts.Apricot, TestProducts.Banana);

        var command = LastCommand();

        Assert.Multiple(() =>
        {
            Assert.That(command.Sql, Does.Contain(EfStore.StringLiteral("Fruit")));
            Assert.That(command.Parameters, Is.Empty);
        });
    }

    private CapturedCommand LastCommand() =>
        EfStore.LastCommand ?? throw new InvalidOperationException("No command was captured.");
}
