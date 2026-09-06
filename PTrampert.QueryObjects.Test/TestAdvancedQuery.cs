using System.Linq.Expressions;
using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test;

internal class TestAdvancedQuery : IQueryObject<TestTarget>
{
    [EqualsQuery]
    public int IntProperty { get; set; }
        
    public Expression<Func<TestTarget, bool>>? BuildQueryExpression()
    {
        return t => t.StringProperty.Contains("Derp");
    }
}