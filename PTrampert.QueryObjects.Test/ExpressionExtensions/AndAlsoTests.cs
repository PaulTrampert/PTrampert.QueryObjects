using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Test.ExpressionExtensions;

[TestFixture]
public class AndAlsoTests
{
    [TestCase(4, false)]
    [TestCase(5, false)]
    [TestCase(6, true)]
    [TestCase(7, true)]
    [TestCase(9, true)]
    [TestCase(10, false)]
    public void AndAlso_CombinesExpressionsWithLogicalAnd(int value, bool expected)
    {
        Expression<Func<int, bool>> left = x => x > 5;
        Expression<Func<int, bool>> right = y => y < 10;
        var combined = left.AndAlso(right);
        var func = combined.Compile();
        Assert.That(func(value), Is.EqualTo(expected));
    }

    [Test]
    public void AndAlso_ReplacesRightParameterWithLeftParameter()
    {
        Expression<Func<int, bool>> left = x => x > 0;
        Expression<Func<int, bool>> right = y => y % 2 == 0;
        var combined = left.AndAlso(right);

        using (Assert.EnterMultipleScope())
        {
            // The parameter in combined should be the same as left's parameter
            Assert.That(combined.Parameters, Has.Count.EqualTo(1));
            Assert.That(combined.Parameters[0], Is.EqualTo(left.Parameters[0]));
            // The body should reference only the left's parameter
            var param = left.Parameters[0];
            var body = combined.Body.ToString();
            Assert.That(body, Does.Contain(param.Name));
        }
    }
}