namespace Application.Core.Api.Result;

/// <summary>
/// Represents a conflict result.
/// </summary>
/// <param name="Message">The error message.</param>
public record Conflict(string Message) : ProblemDetails(Message, 409);