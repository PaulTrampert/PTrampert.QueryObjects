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
        
        Assert.That(expression.ToString(), Is.EqualTo("Param_0 => ((((Param_0.IntProperty == 1) AndAlso (Param_0.AnotherProp > 1)) AndAlso (Param_0.AnotherProp < 4)) AndAlso (Param_0.StringProperty != \"Derp\"))"));
    }
    
    [Test]
    public void BuildQueryExpression_WithAdvancedQueryObject_ReturnsCorrectExpression()
    {
        var query = new TestAdvancedQuery
        {
            IntProperty = 1
        };

        var expression = new QueryExpressionBuilder<TestTarget>().BuildQueryExpression(query);
        
        Assert.That(expression.ToString(), Is.EqualTo("Param_0 => ((Param_0.IntProperty == 1) AndAlso Param_0.StringProperty.Contains(\"Derp\"))"));
    }
}