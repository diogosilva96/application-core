using System.Reflection;

namespace Application.Core.Api.Validation;

/// <summary>
/// Represents a base class for mapping validation failure properties.
/// </summary>
public abstract class ValidationFailurePropertyMapperBase : IValidationFailurePropertyMapper
{
    private readonly List<(Func<string, bool>, string)> _conditionalPropertyMapping = [];
    private readonly Dictionary<string, string> _propertyMapping = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public string this[string propertyName]
    {
        get
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
            
            foreach (var (predicate, mappedPropName) in _conditionalPropertyMapping)
            {
                // ReSharper disable once InvertIf
                if (predicate(propertyName))
                {
                    return mappedPropName;
                }
            }
            // when no mapping is found, return 'unknown property', so that we don't leak internal details
            return _propertyMapping.GetValueOrDefault(propertyName, "unknown property");
        }
    }

    /// <summary>
    /// Configures the mapping for the given <paramref name="propertyNamePredicate"/> and <paramref name="mappedPropertyName"/>.
    /// </summary>
    /// <param name="propertyNamePredicate">The property name predicate to use.</param>
    /// <param name="mappedPropertyName">The mapped property name.</param>
    protected void ConfigureConditionalPropertyMapping(Func<string, bool> propertyNamePredicate, string mappedPropertyName)
    {
        ArgumentNullException.ThrowIfNull(propertyNamePredicate);
        ArgumentException.ThrowIfNullOrWhiteSpace(mappedPropertyName);
        
        _conditionalPropertyMapping.Add((propertyNamePredicate, ToMappedPropertyName(mappedPropertyName)));
    }

    /// <summary>
    /// Configures the mapping for the given <paramref name="propertyName"/> and <paramref name="mappedPropertyName"/>.
    /// </summary>
    /// <param name="propertyName">The property name to map from.</param>
    /// <param name="mappedPropertyName">The property name to map to.</param>
    protected void ConfigurePropertyMapping(string propertyName, string mappedPropertyName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(mappedPropertyName);
        
        _propertyMapping.TryAdd(propertyName, ToMappedPropertyName(mappedPropertyName));
    }

    /// <summary>
    /// Configures the property mappings for the given type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    protected void ConfigurePropertyMappingsForType<T>()
    {
        var type = typeof(T);
        foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            ConfigurePropertyMapping(propertyInfo.Name, propertyInfo.Name);
        }
    }

    private static string ToMappedPropertyName(string mappedPropertyName) => 
        $"{char.ToLowerInvariant(mappedPropertyName[0])}{mappedPropertyName[1..]}";
}