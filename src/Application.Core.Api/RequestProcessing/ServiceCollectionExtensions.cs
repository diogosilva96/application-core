using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Api.RequestProcessing;

/// <summary>
/// Class that contains extension methods for <see cref="IServiceCollection" />
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the necessary services for the API request processor to the given <paramref name="serviceCollection" />
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the services for.</param>
    /// <returns>The given <see cref="IServiceCollection" /> with the added services.</returns>
    public static IServiceCollection AddApiRequestProcessor(this IServiceCollection serviceCollection) =>
        serviceCollection.AddTransient<IApiRequestProcessor, ApiRequestProcessor>();
}