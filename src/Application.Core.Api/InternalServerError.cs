namespace Application.Core.Api;

/// <summary>
/// Represents an InternalServerError result.
/// </summary>
/// <param name="Message">The error message.</param>
// ReSharper disable once RedundantArgumentDefaultValue
public record InternalServerError(string Message) : ProblemDetails(Message, 500);