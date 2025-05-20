namespace Application.Core.Api.Validation;

/// <summary>
/// Represents a validation failure mapper abstraction.
/// </summary>
public interface IValidationFailureMapper
{
    /// <summary>
    /// Maps the given <paramref name="validationFailures"/>.
    /// </summary>
    /// <param name="validationFailures">The validation failures to map from.</param>
    /// <returns>The mapped validation failures.</returns>
    public IReadOnlyDictionary<string, string[]> Map(IReadOnlyDictionary<string, string[]> validationFailures);
}