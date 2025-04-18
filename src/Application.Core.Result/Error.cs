namespace Application.Core.Result;

/// <summary>
/// Represents an error.
/// </summary>
public record Error
{
    /// <summary>
    /// Gets or initializes the message of the error.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Creates an <see cref="Error" /> with the given <paramref name="message" />.
    /// </summary>
    /// <param name="message">The message for the error.</param>
    public Error(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Message = message;
    }
}