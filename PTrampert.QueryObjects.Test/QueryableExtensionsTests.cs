namespace PTrampert.QueryObjects.Test;

public class QueryableExtensionsTests
{
    [Test]
    public void Where_FiltersByTestQueryAttributes()
    {
        var data = new List<TestTarget>
        {
            new TestTarget { IntProperty = 1, AnotherProp = 5, StringProperty = "A" },
            new TestTarget { IntProperty = 2, AnotherProp = 10, StringProperty = "B" },
            new TestTarget { IntProperty = 2, AnotherProp = 15, StringProperty = "Derp" },
            new TestTarget { IntProperty = 3, AnotherProp = 20, StringProperty = "C" }
        };

        var query = new TestQuery
        {
            IntProperty = 2,
            AnotherPropLowerLimit = 9,
            AnotherPropUpperLimit = 16,
            StringProperty = "B"
        };

        var result = data.Where(query).ToList();

        Assert.That(result, Is.EqualTo(new [] 
        {
            new TestTarget { IntProperty = 2, AnotherProp = 15, StringProperty = "Derp" }
        }));
    }

    [Test]
    public void Where_UsesAdvancedQueryExpression()
    {
        var data = new List<TestTarget>
        {
            new TestTarget { IntProperty = 1, AnotherProp = 5, StringProperty = "A" },
            new TestTarget { IntProperty = 2, AnotherProp = 10, StringProperty = "Derp" },
            new TestTarget { IntProperty = 2, AnotherProp = 20, StringProperty = "DerpX" }
        };

        var query = new TestAdvancedQuery { IntProperty = 2 };

        var result = data.Where(query).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(t => t.StringProperty.Contains("Derp")), Is.True);
    }
}