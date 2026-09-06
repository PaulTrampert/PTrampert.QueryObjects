using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Test;

internal record TestQuery
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