using System.Text.RegularExpressions;
using Application.Core.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Core.Api.Validation;

/// <summary>
/// Represents a validation failure mapper component.
/// </summary>
public class ValidationFailureMapper(IHttpContextAccessor httpContextAccessor, ILogger<ValidationFailureMapper> logger) : IValidationFailureMapper
{
    /// <summary>
    /// Maps the given <paramref name="validationFailures" />.
    /// </summary>
    /// <param name="validationFailures">The validation failures to map from.</param>
    /// <returns>The mapped validation failures.</returns>
    public IReadOnlyDictionary<string, string[]> Map(IReadOnlyDictionary<string, string[]> validationFailures)
    {
        if (httpContextAccessor.HttpContext is null) return validationFailures;

        var propertyMapperBinding = httpContextAccessor.HttpContext.GetEndpoint()?.Metadata.GetMetadata<ValidationFailurePropertyMapperBinding>();
        if (propertyMapperBinding is null)
        {
            logger.LogWarning("No {ValidationFailurePropertyMapperBindingType} found for endpoint {Endpoint}.", nameof(ValidationFailurePropertyMapperBinding), EndpointDescriptors.For(httpContextAccessor.HttpContext.Request));
            return validationFailures;
        }

        var propertyMapper = httpContextAccessor.HttpContext.RequestServices.GetServices<IValidationFailurePropertyMapper>().SingleOrDefault(m => m.GetType() == propertyMapperBinding.MapperType);
        if (propertyMapper is null)
        {
            throw new InvalidOperationException(
                $"Could not find '{nameof(IValidationFailurePropertyMapper)}' for type '{propertyMapperBinding.MapperType}' in the DI container - maybe the mapper is not registered in the DI container.");
        }
        
        return MapValidationFailures(validationFailures, propertyMapper);
    }

    private static Dictionary<string, string[]> MapValidationFailures(IReadOnlyDictionary<string, string[]> validationFailures, IValidationFailurePropertyMapper propertyMapper)
    {
        var mappedFailures = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        foreach (var (propertyName, failures) in validationFailures)
        {
            var mappedPropertyName = propertyMapper[propertyName];
            var mappedErrorMessages = failures.Select(f => Regex.Replace(f, @$"\b{propertyName}\b", mappedPropertyName, RegexOptions.IgnoreCase)).ToArray();
            if (mappedFailures.TryGetValue(mappedPropertyName, out var existingErrorMessages))
            {
                mappedFailures[mappedPropertyName] = mappedErrorMessages.Concat(existingErrorMessages).ToArray();
                continue;
            }

            mappedFailures.Add(mappedPropertyName, mappedErrorMessages);
        }

        return mappedFailures;
    }
}