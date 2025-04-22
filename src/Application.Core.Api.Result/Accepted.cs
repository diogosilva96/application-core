namespace Application.Core.Api.Result;

/// <summary>
/// Represents a accepted result.
/// </summary>
/// <param name="Value">The optional value</param>
public record Accepted(object? Value = null) : Successful(Value);