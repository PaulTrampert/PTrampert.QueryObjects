using System.Linq.Expressions;
using PTrampert.QueryObjects.Attributes;
using PTrampert.QueryObjects.Internals;

namespace PTrampert.QueryObjects;

public class QueryExpressionBuilderTests
{
    class TestTarget
    {
        public int AnotherProp { get; set; }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; } = string.Empty;
    }

    class TestQuery
    {
        [EqualsQuery]
        public int IntProperty { get; set; }
        
        [GreaterThanQuery(nameof(TestTarget.AnotherProp))]
        public int AnotherPropLowerLimit { get; set; }
        
        [LessThanQuery(nameof(TestTarget.AnotherProp))]
        public int AnotherPropUpperLimit { get; set; }
        
        [NotEqualsQuery]
        public string StringProperty { get; set; } = "Derp";
    }
    
    class TestAdvancedQuery : IQueryObject<TestTarget>
    {
        [EqualsQuery]
        public int IntProperty { get; set; }
        
        public Expression<Func<TestTarget, bool>>? BuildQueryExpression()
        {
            return t => t.StringProperty.Contains("Derp");
        }
    }
    
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