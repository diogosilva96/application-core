using System.Text.Json;
using Application.Core.Result;

namespace Application.Core.Api.Result;

/// <summary>
/// Represents a problem details result.
/// </summary>
public record ProblemDetails : IError
{
    /// <summary>
    /// Creates a new instance of <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="status">The status.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Exception thrown when the <paramref name="message"/> is not specified,
    /// or when the <paramref name="status"/> is not in the expected problem details range.
    /// </exception>
    public ProblemDetails(string message, int status = 500)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentOutOfRangeException.ThrowIfLessThan(status, 400);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(status, 600);
        
        Message = message;
        Status = status;
    }
    
    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; }
    
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
    public IReadOnlyDictionary<string, object?> Extensions
    {
        get => _extensions;
        init
        {
            var extensions = new Dictionary<string, object?>(_extensions, StringComparer.OrdinalIgnoreCase);
            foreach (var (key, val) in value)
            {
                if (!extensions.TryAdd(key, val))
                {
                    throw new InvalidOperationException($"The key '{key}' already exists in the extensions.");
                }
            }
            _extensions = extensions;
        }
    }

    private readonly Dictionary<string, object?> _extensions = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Adds and entry to the extension dictionary.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="InvalidOperationException">
    /// Exception thrown when there is already a key for the given <paramref name="key"/>.
    /// </exception>
    protected void AddExtension(string key, object? value)
    {
        if (!_extensions.TryAdd(key, value))
        {
            throw new InvalidOperationException($"The key '{key}' already exists in the extensions.");
        }
    }
    
    /// <inheritdoc />
    public string ToDetailedErrorMessage()
    {
        var message = $"{Message}.";

        if (Type is not null)
        {
            message += $" Type: {Type}.";
        }

        if (Title is not null)
        {
            message += $" Title: {Title}.";
        }

        if (Detail is not null)
        {
            message += $" Detail: {Detail}.";
        }

        if (Instance is not null)
        {
            message += $" Instance: {Instance}.";
        }

        message += $" Status: {Status}.";

        // ReSharper disable once InvertIf
        if (Extensions.Count > 0)
        {
            try
            {
                message += $"Extensions: {JsonSerializer.Serialize(Extensions)}";
            }
            catch (Exception)
            {
                message += "Extensions: <error serializing extensions>.";
            }
        }

        return message;
    }
}