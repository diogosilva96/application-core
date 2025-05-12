using FluentValidation.Results;

namespace Application.Core.Api.Result;

/// <summary>
/// Represents a bad request result.
/// </summary>
public record BadRequest : ProblemDetails
{
    /// <summary>
    /// Creates a new instance of <see cref="BadRequest"/>.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="validationFailures">The validation failures.</param>
    public BadRequest(string message, IEnumerable<ValidationFailure> validationFailures) : base(message, 400)
    {
        ArgumentNullException.ThrowIfNull(validationFailures);

        var errors = validationFailures.GroupBy(f => f.PropertyName)
                                       .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());
        
        AddExtension(ProblemDetailsExtensionKeys.Errors, errors);
    }

    /// <summary>
    /// Gets the errors.
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors => 
        (IReadOnlyDictionary<string, string[]>)(Extensions[ProblemDetailsExtensionKeys.Errors] ?? throw new InvalidOperationException("No errors found.")) ;
}