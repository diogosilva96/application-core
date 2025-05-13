using Microsoft.AspNetCore.Builder;

namespace Application.Core.Api.Validation;

/// <summary>
/// Class that contains extension methods for <see cref="RouteHandlerBuilder" />.
/// </summary>
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="ValidationFailurePropertyMapperBase" /> to the given <paramref name="builder" />.
    /// </summary>
    /// <param name="builder">The builder to add the property mapper to.</param>
    /// <typeparam name="TMapper">The mapper type.</typeparam>
    /// <returns>The configured <see cref="RouteHandlerBuilder"/>.</returns>
    public static RouteHandlerBuilder WithValidationFailurePropertyMapper<TMapper>(this RouteHandlerBuilder builder) 
        where TMapper : class, IValidationFailurePropertyMapper
    {
        return builder.WithMetadata(new ValidationFailurePropertyMapperBinding(typeof(TMapper)));
    }
}