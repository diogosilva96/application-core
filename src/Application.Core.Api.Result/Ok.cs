namespace Application.Core.Api.Result;

/// <summary>
/// Represents an OK result.
/// </summary>
/// <param name="Value">The optional value.</param>
public record Ok(object? Value = null) : Successful(Value);