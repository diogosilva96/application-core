namespace Application.Core.Api.Result;

/// <summary>
/// Represents a accepted result.
/// </summary>
/// <param name="Value">The optional value</param>
/// <param name="Uri">The optional uri.</param>
public record Accepted(object? Value = null, Uri? Uri = null) : Success;