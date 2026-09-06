namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// A SQL command as it was actually sent to the database.
/// </summary>
/// <param name="Sql">The command text, before the provider substitutes any parameters.</param>
/// <param name="Parameters">The parameter values sent alongside <paramref name="Sql"/>, keyed by parameter name.</param>
public record CapturedCommand(string Sql, IReadOnlyDictionary<string, object?> Parameters);
