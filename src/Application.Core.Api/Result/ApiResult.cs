using Application.Core.Result;

namespace Application.Core.Api.Result;

/// <summary>
/// Represents an api result.
/// </summary>
public record ApiResult : Result<Success, ProblemDetails>
{
    /// <summary>
    /// Creates a new instance of <see cref="ApiResult"/> with the given <paramref name="success"/>.
    /// </summary>
    /// <param name="success">The success result.</param>
    public ApiResult(Success success) : base(success)
    { }

    /// <summary>
    /// Creates a new instance of <see cref="ApiResult"/> with the given <paramref name="problemDetails"/>.
    /// </summary>
    /// <param name="problemDetails">The problem details.</param>
    public ApiResult(ProblemDetails problemDetails) : base(problemDetails)
    { }
    
    public static implicit operator ApiResult(Success success) => new(success);
    
    public static implicit operator ApiResult(ProblemDetails problemDetails) => new(problemDetails);
}