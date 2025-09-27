---
title: Implementing IQueryObject<T>
---

# Implementing IQueryObject<T>


`IQueryObject<T>` is an interface that allows you to define custom query logic for filtering `IQueryable<T>` sources. Implementing this interface gives you full control over how your query object translates its properties into a LINQ expression.

> **Note:** Implementing `IQueryObject<T>` does not replace any query attributes (annotations) you use on properties. Instead, the expression returned by `BuildQueryExpression()` is logically AND'ed with the expressions generated from annotated properties. This allows you to combine custom logic with attribute-based filtering seamlessly.

## Basic Implementation


To implement `IQueryObject<T>`, create a class that implements the interface and its `BuildQueryExpression()` method:

```csharp
using PTrampert.QueryObjects;
using System;
using System.Linq.Expressions;

public class UserQuery : IQueryObject<User>
{
    public int? Id { get; set; }
    public string Name { get; set; }

    public Expression<Func<User, bool>> BuildQueryExpression()
    {
        // Build your expression here
    }
}
```

## Using ExpressionExtensions

The library provides `ExpressionExtensions` to help you build complex expressions more easily. These helpers allow you to combine, chain, and conditionally add predicates to your expression.



For example:

```csharp
using PTrampert.QueryObjects.ExpressionExtensions;

public Expression<Func<User, bool>> BuildQueryExpression()
{
    Expression<Func<User, bool>> expr = u => true;

    if (Id.HasValue)
        expr = expr.AndAlso(u => u.Id == Id.Value);

    if (!string.IsNullOrEmpty(Name))
        expr = expr.AndAlso(u => u.Name.Contains(Name));

    // Example of using OrElse:
    // expr = expr.OrElse(u => u.IsActive);

    return expr;
}
```

Here, `AndAlso` combines two expressions with a logical AND, and `OrElse` combines with a logical OR. These helpers make it easy to build complex, composable queries.

## Summary

- Implement `IQueryObject<T>` to define custom query logic.
- Use `ExpressionExtensions` to simplify building and combining expressions.
- Return an `Expression<Func<T, bool>>` that represents your filter criteria.

For more details, see the API documentation for `IQueryObject<T>` and `ExpressionExtensions`.
