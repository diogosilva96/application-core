using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the mediator service(s) to the given <paramref name="serviceCollection" />.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="configureMediator">The action for configuring the mediator.</param>
    /// <returns>The <see cref="IServiceCollection" /> with the added service(s).</returns>
    public static IServiceCollection AddMediator(this IServiceCollection serviceCollection, Action<MediatorConfigurator>? configureMediator = null)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var configurator = new MediatorConfigurator(serviceCollection);
        configureMediator?.Invoke(configurator);
        
        return serviceCollection.AddTransient<IMediator, Internal.Mediator>();
    }
}