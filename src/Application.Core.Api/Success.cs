namespace Application.Core.Api;

/// <summary>
/// Represents a successful result.
/// </summary>
/// <param name="Value">The optional value.</param>
// TODO: check how we'd rather pass the headers in case we need to add custom headers to a response.
public record Success(object? Value = null);