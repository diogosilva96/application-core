using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the mediator service(s) to the given <paramref name="serviceCollection"/>.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the added service(s).</returns>
    public static IServiceCollection AddMediator(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        
        return serviceCollection.AddTransient<IMediator, Internal.Mediator>();
    }
}