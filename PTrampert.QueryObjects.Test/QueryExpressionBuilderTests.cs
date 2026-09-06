using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects.Test;

public class QueryExpressionBuilderTests
{
    [Test]
    public void BuildQueryExpression_WithSimpleQueryObject_ReturnsCorrectExpression()
    {
        var query = new TestQuery
        {
            IntProperty = 1,
            AnotherPropLowerLimit = 1,
            AnotherPropUpperLimit = 4,
            StringProperty = "Derp"
        };

        var expression = new QueryExpressionBuilder<TestTarget>().BuildQueryExpression(query);

        // Query values are referenced as member accesses on the query object rather than inlined as
        // constants, so that ORMs lift them into query parameters.
        const string q = "value(PTrampert.QueryObjects.Test.TestQuery)";
        Assert.That(expression.ToString(), Is.EqualTo(
            $"Param_0 => ((((Param_0.IntProperty == {q}.IntProperty) "
            + $"AndAlso (Param_0.AnotherProp > {q}.AnotherPropLowerLimit)) "
            + $"AndAlso (Param_0.AnotherProp < {q}.AnotherPropUpperLimit)) "
            + $"AndAlso (Param_0.StringProperty != {q}.StringProperty))"));
    }
    
    [Test]
    public void BuildQueryExpression_WithAdvancedQueryObject_ReturnsCorrectExpression()
    {
        var query = new TestAdvancedQuery
        {
            IntProperty = 1
        };

        var expression = new QueryExpressionBuilder<TestTarget>().BuildQueryExpression(query);

        const string q = "value(PTrampert.QueryObjects.Test.TestAdvancedQuery)";
        Assert.That(expression.ToString(), Is.EqualTo(
            $"Param_0 => ((Param_0.IntProperty == {q}.IntProperty) AndAlso Param_0.StringProperty.Contains(\"Derp\"))"));
    }
}
