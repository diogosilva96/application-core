namespace Application.Core.Api.Result;

/// <summary>
/// Represents a not found result.
/// </summary>
/// <param name="Message">The error message.</param>
public record NotFound(string Message) : ProblemDetails(Message, 404);