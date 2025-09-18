
---
title: Introduction to PTrampert.QueryObjects
---

# Introduction to PTrampert.QueryObjects

**PTrampert.QueryObjects** is a .NET library designed to simplify and standardize the process of building dynamic LINQ queries using strongly-typed query objects. It enables developers to express complex filtering logic in a clean, maintainable, and reusable way.

## Why Use Query Objects?

Traditional query building often involves manually composing LINQ expressions or handling many optional parameters. Query objects allow you to:

- Encapsulate query logic in dedicated classes
- Use attributes to declaratively specify how each property should be translated into a query
- Reduce boilerplate code and improve readability
- Make queries easier to test and maintain

## Key Features

- Attribute-based query mapping for properties
- Extension methods for applying query objects to `IQueryable<T>`
- Support for a wide range of comparison and filtering scenarios
- Works seamlessly with Entity Framework and other LINQ providers

## Typical Use Cases

- Filtering data in repositories and services
- Building search APIs with flexible query parameters
- Creating reusable query logic for business rules

## How It Works

1. Define a query object class with properties representing filter criteria.
2. Decorate properties with query attributes to specify their behavior.
3. Use the provided extension methods to apply the query object to your data source.

For a step-by-step guide, see the [Getting Started](./getting-started.md) page.

## Learn More

- [Getting Started](./getting-started.md)
- [API Reference](../api/)

---