using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Represents the mediator configuration.
/// </summary>
public class MediatorConfiguration
{
    private static readonly Type _genericHandlerBehaviorAbstractionType = typeof(IHandlerBehavior<,>);
    private static readonly Type _genericHandlerAbstractionType = typeof(IHandler<,>);
    private readonly List<ServiceDescriptor> _servicesToRegister = [];
    
    /// <summary>
    /// Gets the configured services to register.
    /// </summary>
    public IReadOnlyList<ServiceDescriptor> ServicesToRegister => _servicesToRegister;

    /// <summary>
    /// Gets or sets the lifetime of <see cref="ISender"/>. Defaults to <see cref="ServiceLifetime.Transient"/>.
    /// </summary>
    public ServiceLifetime SenderLifetime { get; set; } = ServiceLifetime.Transient;
    

    /// <summary>
    /// Adds the specified <paramref name="behaviorType" />.
    /// </summary>
    /// <param name="behaviorType">The behavior type to add.</param>
    /// <param name="serviceLifetime">The service life time for the behavior being registered.</param>
    /// <remarks>The order of the behaviors being registered will be the order in which they will execute.</remarks>
    /// <returns>The <see cref="MediatorConfiguration" />.</returns>
    /// <exception cref="ArgumentException">
    /// Exception thrown when the <paramref name="behaviorType" /> is not a behavior type.
    /// </exception>
    public MediatorConfiguration AddBehavior(Type behaviorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(behaviorType);
        if (!IsBehaviorType(behaviorType))
            throw new ArgumentException($"The specified type '{behaviorType.Name}' is not a handler behavior type.", nameof(behaviorType));
        
        _servicesToRegister.Add(new(_genericHandlerBehaviorAbstractionType, behaviorType, serviceLifetime));

        return this;
    }

    /// <summary>
    /// Registers the handlers in the specified <paramref name="assemblies" />.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The <see cref="MediatorConfiguration" /> builder with the added handlers.</returns>
    public MediatorConfiguration AddHandlersFromAssemblies(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0) throw new ArgumentException("At least one assembly must be specified.", nameof(assemblies));

        foreach (var (interfaceType, implementationType) in ScanHandlerTypesFromAssemblies(assemblies))
        {
            _servicesToRegister.Add(new(interfaceType, implementationType, ServiceLifetime.Transient));
        }

        return this;
    }

    /// <summary>
    /// Registers the handlers contained in the assembly of the specified type <see cref="T:T" />.
    /// </summary>
    /// <typeparam name="T">The type containing the assembly to register the handlers for.</typeparam>
    /// <returns>The <see cref="MediatorConfiguration" /> with the added handlers.</returns>
    public MediatorConfiguration AddHandlersFromAssemblyContaining<T>()
    {
        var assembly = typeof(T).Assembly;

        return AddHandlersFromAssemblies(assembly);
    }

    private static (Type InterfaceType, Type ImplementationType)[] ScanHandlerTypesFromAssemblies(params Assembly[] assemblies)
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