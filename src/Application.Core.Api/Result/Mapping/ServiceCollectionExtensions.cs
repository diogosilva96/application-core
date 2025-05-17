using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Api.Result.Mapping;

/// <summary>
/// Class that contains extension methods for <see cref="IServiceCollection" />
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the necessary services for the API result mapping to the given <paramref name="serviceCollection" />
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the services for.</param>
    /// <returns>The given <see cref="IServiceCollection" /> with the API result mapping services configured.</returns>
    public static IServiceCollection AddApiResultMapping(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        
        return serviceCollection.AddTransient<IApiResultMapper, ApiResultMapper>()
                         .AddHttpContextAccessor();
    }
}