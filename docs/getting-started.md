---
title: Getting Started with PTrampert.QueryObjects
---

# Getting Started with PTrampert.QueryObjects

Welcome to **PTrampert.QueryObjects**! This library provides a flexible way to build LINQ queries in .NET using strongly-typed query objects and attributes. It is ideal for filtering, searching, and querying data in repositories, APIs, or services.

## Installation


Install the NuGet package:

```sh
dotnet add package PTrampert.QueryObjects
```

## Basic Usage

1. **Define a Query Object**

Create a class that represents your query parameters. Decorate properties with query attributes:

```csharp
using PTrampert.QueryObjects.Attributes;

public class UserQuery
{
	[EqualsQuery]
	public int? Id { get; set; }

	[StringContainsQuery]
	public string Name { get; set; }

	[GreaterThanOrEqualQuery]
	public int? Age { get; set; }
}
```


2. **Apply the Query Object to a Data Source**

Use the `Where` extension method to filter an `IQueryable<T>`:

```csharp
using PTrampert.QueryObjects;

var query = new UserQuery { Name = "Alice", Age = 18 };
var filteredUsers = dbContext.Users.Where(query);
```


## Query Values and Parameterization

Query values are not baked into the generated expression tree as literal constants. Instead, each attribute
emits a member access on the query object itself -- the same shape the C# compiler produces for a captured
variable:

```csharp
// x => x.Name == queryObject.Name
```

ORMs such as Entity Framework Core lift that shape into a SQL parameter, so the SQL produced for a given query
object type is identical regardless of the values supplied. That keeps EF Core's compiled query cache and the
database's query plan cache effective. A literal constant, by contrast, is inlined into the SQL, producing
different SQL text for every value.

If you need the value inlined instead -- for example when a predicate runs over badly skewed data and you would
rather the database build a plan for the specific value than reuse one built for a previous value -- set
`InlineValue` on the attribute:

```csharp
public class UserQuery
{
    [EqualsQuery(InlineValue = true)]
    public bool? IsDeleted { get; set; }
}
```

`InlineValue` is available on every query attribute and defaults to `false`.

## Supported Query Attributes

For a complete and up-to-date list of supported query attributes, please refer to the [API documentation for PTrampert.QueryObjects.Attributes](../api/PTrampert.QueryObjects.Attributes.yml).

## Resources

- [API Reference](../api/)
- [Introduction](./introduction.md)

---
For more details, see the API documentation and explore the source code on GitHub.