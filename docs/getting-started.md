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


## Supported Query Attributes

For a complete and up-to-date list of supported query attributes, please refer to the [API documentation for PTrampert.QueryObjects.Attributes](../api/PTrampert.QueryObjects.Attributes.yml).

## Resources

- [API Reference](../api/)
- [Introduction](./introduction.md)

---
For more details, see the API documentation and explore the source code on GitHub.