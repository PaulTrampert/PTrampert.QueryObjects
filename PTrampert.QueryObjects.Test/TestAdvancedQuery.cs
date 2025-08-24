using System.Linq.Expressions;
using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects;

internal record TestAdvancedQuery : IQueryObject<TestTarget>
{
    [EqualsQuery]
    public int IntProperty { get; set; }
        
    public Expression<Func<TestTarget, bool>>? BuildQueryExpression()
    {
        return t => t.StringProperty.Contains("Derp");
    }
}