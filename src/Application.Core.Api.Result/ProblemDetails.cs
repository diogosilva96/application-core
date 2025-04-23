using Application.Core.Result;

namespace Application.Core.Api.Result;

/// <summary>
/// Represents a problem details result.
/// </summary>
public record ProblemDetails : Error
{
    /// <summary>
    /// Creates a new instance of <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="status">The status.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Exception thrown when the <paramref name="status"/> is not in the expected problem details range.
    /// </exception>
    public ProblemDetails(string message, int status = 500) : base(message)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(status, 400);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(status, 600);
        Status = status;
    }
    /// <summary>
    /// Gets or initializes the type.
    /// </summary>
    public string? Type { get; init; }
    
    /// <summary>
    /// Gets or initializes the title.
    /// </summary>
    public string? Title { get; init; }
    
    /// <summary>
    /// Gets or initializes the detail.
    /// </summary>
    public string? Detail { get; init; }
    
    /// <summary>
    /// Gets or initializes the instance.
    /// </summary>
    public string? Instance { get; init; }
    
    /// <summary>
    /// The status code.
    /// </summary>
    public int Status { get; }

    /// <summary>
    /// The extensions.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Extensions { get; init; } = new Dictionary<string, object?>();
}