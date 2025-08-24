namespace PTrampert.QueryObjects;

internal record TestTarget
{
    public int AnotherProp { get; init; }
    public int IntProperty { get; init; }
    public string StringProperty { get; init; } = string.Empty;
}