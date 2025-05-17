using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Api.Validation;

/// <summary>
/// Class that contains extension methods for <see cref="IServiceCollection" />
/// </summary>
public static class ServiceCollectionExtensions
{
    private static readonly Type _mapperAbstractionType = typeof(IValidationFailurePropertyMapper);

    /// <summary>
    /// Adds the required services for validation failure mapping to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="serviceCollection"> The service collection to add the services for.</param>
    /// <param name="assemblies">The assemblies containing the implementation for the <see cref="IValidationFailurePropertyMapper" />.</param>
    /// <returns>The service collection with the added services.</returns>
    public static IServiceCollection AddValidationFailureMapping(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        
        foreach (var implementationType in ScanPropertyMapperImplementationTypesFromAssemblies(assemblies))
        {
            serviceCollection.AddSingleton(_mapperAbstractionType, implementationType);
        }

        return serviceCollection.AddScoped<IValidationFailureMapper, ValidationFailureMapper>()
                                .AddHttpContextAccessor();
    }

    private static Type[] ScanPropertyMapperImplementationTypesFromAssemblies(params Assembly[] assemblies) =>
        assemblies.SelectMany(a => a.GetTypes().Where(t => t is { IsAbstract: false, IsInterface: false } &&
                                                           t.GetInterfaces().Any(it => it == _mapperAbstractionType)))
                  .Select(t => t)
                  .ToArray();
}