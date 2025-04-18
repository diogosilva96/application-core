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
    /// <exception cref="ArgumentException">
    /// Exception thrown when the <paramref name="message"/> is not specified.
    /// </exception>
    public Error(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Message = message;
    }
}