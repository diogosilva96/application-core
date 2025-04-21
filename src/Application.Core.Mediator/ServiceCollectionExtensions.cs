using System.Collections.Concurrent;
using System.Reflection;
using Application.Core.Mediator.Internal;
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
    public static IServiceCollection AddMediator(this IServiceCollection serviceCollection, Action<MediatorConfiguration>? configureMediator = null)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var configuration = new MediatorConfiguration();
        configureMediator?.Invoke(configuration);

        foreach (var service in configuration.ServicesToRegister)
        {
            serviceCollection.Add(service);
        }

        serviceCollection.AddKeyedSingleton<ConcurrentDictionary<Type, MethodInfo>>(ServiceKeys.SenderMethodCache);
        serviceCollection.Add(new(typeof(ISender), typeof(Sender), configuration.SenderLifetime));
        
        return serviceCollection;
    }
}