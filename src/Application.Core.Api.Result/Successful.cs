namespace Application.Core.Api.Result;

/// <summary>
/// Represents a successful result.
/// </summary>
/// <param name="Value">The optional value.</param>
public record Successful(object? Value = null);