namespace Application.Core.Api.Result;

/// <summary>
/// Represents a forbidden result.
/// </summary>
/// <param name="Message">The error message.</param>
public record Forbidden(string Message) : ProblemDetails(Message, 403);