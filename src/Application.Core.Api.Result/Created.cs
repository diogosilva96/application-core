namespace Application.Core.Api.Result;

/// <summary>
/// Represents a created result.
/// </summary>
/// <param name="Value">The optional value</param>
/// <param name="Uri">The optional uri.</param>
public record Created(object? Value = null, Uri? Uri = null) : Successful(Value);