using Application.Core.Result;

namespace Application.Core.Api.Result;

/// <summary>
/// Represents an api result.
/// </summary>
public record ApiResult : Result<Success, ProblemDetails>
{
    /// <summary>
    /// Creates a new instance of <see cref="ApiResult"/> with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The success result.</param>
    public ApiResult(Success value) : base(value)
    { }

    /// <summary>
    /// Creates a new instance of <see cref="ApiResult"/> with the given <paramref name="problemDetails"/>.
    /// </summary>
    /// <param name="problemDetails">The problem details.</param>
    public ApiResult(ProblemDetails problemDetails) : base(problemDetails)
    { }
    
    public static implicit operator ApiResult(Success value) => new(value);
    
    public static implicit operator ApiResult(ProblemDetails problemDetails) => new(problemDetails);
}