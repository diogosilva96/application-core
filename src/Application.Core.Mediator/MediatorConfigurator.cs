using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Represents the mediator configurator.
/// </summary>
public class MediatorConfigurator
{
    private static readonly Type _genericHandlerBehaviorAbstractionType = typeof(IHandlerBehavior<,>);
    private static readonly Type _genericHandlerAbstractionType = typeof(IHandler<,>);
    private readonly IServiceCollection _serviceCollection;

    internal MediatorConfigurator(IServiceCollection serviceCollection) => _serviceCollection = serviceCollection;

    /// <summary>
    /// Adds the specified <paramref name="behaviorType" />.
    /// </summary>
    /// <param name="behaviorType">The behavior type to add.</param>
    /// <remarks>The order of the behaviors being registered will be the order in which they will execute.</remarks>
    /// <returns>The <see cref="MediatorConfigurator" />.</returns>
    /// <exception cref="ArgumentException">
    /// Exception thrown when the <paramref name="behaviorType" /> is not a behavior type.
    /// </exception>
    public MediatorConfigurator AddBehavior(Type behaviorType)
    {
        ArgumentNullException.ThrowIfNull(behaviorType);
        if (!IsBehaviorType(behaviorType))
            throw new ArgumentException($"The specified type '{behaviorType.Name}' is not a handler behavior type.", nameof(behaviorType));

        _serviceCollection.AddTransient(_genericHandlerBehaviorAbstractionType, behaviorType);

        return this;
    }

    /// <summary>
    /// Registers the handlers in the specified <paramref name="assemblies" />.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The <see cref="MediatorConfigurator" /> builder with the added handlers.</returns>
    public MediatorConfigurator AddHandlersFromAssemblies(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0) throw new ArgumentException("At least one assembly must be specified.", nameof(assemblies));

        foreach (var (abstractionType, implementationType) in ScanHandlerTypesFromAssemblies(assemblies))
        {
            _serviceCollection.AddTransient(abstractionType, implementationType);
        }

        return this;
    }

    /// <summary>
    /// Registers the handlers contained in the assembly of the specified type <see cref="T:T" />.
    /// </summary>
    /// <typeparam name="T">The type containing the assembly to register the handlers for.</typeparam>
    /// <returns>The <see cref="MediatorConfigurator" /> with the added handlers.</returns>
    public MediatorConfigurator AddHandlersFromAssemblyContaining<T>()
    {
        var assembly = typeof(T).Assembly;

        return AddHandlersFromAssemblies(assembly);
    }

    private static (Type AbstractionType, Type ImplementationType)[] ScanHandlerTypesFromAssemblies(params Assembly[] assemblies)
    {
        return assemblies.SelectMany(a => a.GetTypes().Where(t => t is { IsAbstract: false, IsInterface: false } && 
                                                                  t.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == _genericHandlerAbstractionType)))
                         .Select(t => new ValueTuple<Type, Type>(t.GetInterfaces().First(it => it.IsGenericType && it.GetGenericTypeDefinition() == _genericHandlerAbstractionType), t))
                         .ToArray();
    }

    private static bool IsBehaviorType(Type behaviorType) =>
        behaviorType.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .Any(t => t.GetGenericTypeDefinition() == _genericHandlerBehaviorAbstractionType);
}