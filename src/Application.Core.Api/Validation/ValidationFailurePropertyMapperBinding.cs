namespace Application.Core.Api.Validation;

/// <summary>
/// Represents a binding for a validation failure property mapper.
/// </summary>
public class ValidationFailurePropertyMapperBinding
{
    /// <summary>
    /// The mapper type.
    /// </summary>
    public Type MapperType { get; }
    
    /// <summary>
    /// Creates a new instance of <see cref="ValidationFailurePropertyMapperBinding"/>.
    /// </summary>
    /// <param name="validationFailureMapperType">The validation failure mapper type.</param>
    /// <exception cref="ArgumentException">
    /// Exception thrown when the given <paramref name="validationFailureMapperType"/> is not a valid mapper type.
    /// </exception>
    public ValidationFailurePropertyMapperBinding(Type validationFailureMapperType)
    {
        if (!validationFailureMapperType.GetInterfaces().Contains(typeof(IValidationFailurePropertyMapper))) throw new ArgumentException($"The type '{validationFailureMapperType.Name}' must implement {nameof(IValidationFailurePropertyMapper)}", nameof(validationFailureMapperType));
        MapperType = validationFailureMapperType;
    }
}