namespace Application.Core.Api.Result;

/// <summary>
/// Represents an unauthorized result.
/// </summary>
/// <param name="Message">The error message.</param>
public record Unauthorized(string Message) : ProblemDetails(Message, 401);