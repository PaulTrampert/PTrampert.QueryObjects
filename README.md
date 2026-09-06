# PTrampert.QueryObjects

[![NuGet Version](https://img.shields.io/nuget/v/PTrampert.QueryObjects.svg?style=flat-square)](https://www.nuget.org/packages/PTrampert.QueryObjects)

Attribute-based query objects for LINQ/IQueryable in .NET

## Overview

PTrampert.QueryObjects is a .NET library that enables you to define query objects using attributes, making it easy to build dynamic, type-safe queries for `IQueryable<T>` sources. By annotating properties on your query object, you can declaratively specify how each property should filter your data, and then apply the query object to any `IQueryable<T>` using a simple extension method.

## Features
- Attribute-based query object filtering
- Supports custom query logic via `IQueryObject<T>`
- Works with any `IQueryable<T>` (e.g. Entity Framework, MongoDB) or `IEnumerable<T>`
- Emits query values as parameterizable member accesses rather than inline constants, so ORM and database query
  caches stay effective (opt out per property with `InlineValue = true`)

## Example Usage

Suppose you have a target class and a query object:

```csharp
public class Person
{
    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class PersonQuery
{
    [EqualsQuery]
    public int? Age { get; set; }

    [StringContainsQuery]
    public string? Name { get; set; }
}
```

You can use your query object to filter an `IQueryable<Person>`:

```csharp
using PTrampert.QueryObjects;

var people = new List<Person>
{
    new Person { Age = 30, Name = "Alice" },
    new Person { Age = 40, Name = "Bob" },
    new Person { Age = 30, Name = "Caroline" }
};

var query = new PersonQuery { Age = 30, Name = "li" };

var filtered = people.Where(query).ToList();
// filtered contains Alice and Caroline
```

Full documentation can be found [here](https://paultrampert.github.io/PTrampert.QueryObjects/)

## Verified Providers

`PTrampert.QueryObjects.Integration.Test` runs one shared suite against real databases, so every query attribute is
proven to translate and execute server side on:

- Entity Framework Core backed by SQL Server
- Entity Framework Core backed by PostgreSQL
- The MongoDB C# driver's LINQ provider

The suite uses [Testcontainers](https://dotnet.testcontainers.org/) to start the databases, so running it requires a
working Docker daemon. `dotnet test` on the solution runs it alongside the unit tests; to run a single provider, filter
by its category:

```sh
dotnet test --filter "TestCategory=PostgreSql"   # or SqlServer, or MongoDb
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.md) file for details.

