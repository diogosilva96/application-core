namespace Application.Core.Api.Validation;

/// <summary>
/// Represents a validation failure property mapper abstraction.
/// </summary>
public interface IValidationFailurePropertyMapper
{
    /// <summary>
    /// Maps the given <paramref name="propertyName"/> to its mapped property name.
    /// </summary>
    /// <param name="propertyName">The property name to map.</param>
    public string this[string propertyName] { get; }
}