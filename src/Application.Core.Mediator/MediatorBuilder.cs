using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Represents the mediator configuration.
/// </summary>
public class MediatorBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private static readonly Type _handlerBehaviorAbstractionType = typeof(IHandlerBehavior<,>);

    internal MediatorBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
    
    /// <summary>
    /// Adds the specified <paramref name="behaviorType"/>.
    /// </summary>
    /// <param name="behaviorType">The behavior type to add.</param>
    /// <returns>The <see cref="MediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Exception thrown when the <paramref name="behaviorType"/> is not a behavior type.
    /// </exception>
    public MediatorBuilder AddBehavior(Type behaviorType)
    {
        ArgumentNullException.ThrowIfNull(behaviorType);
        if (!IsBehaviorType(behaviorType)) throw new ArgumentException($"The specified type '{behaviorType.Name}' is not a handler behavior type.", nameof(behaviorType));
        
        _serviceCollection.AddTransient(_handlerBehaviorAbstractionType, behaviorType);

        return this;
    }

    private static bool IsBehaviorType(Type behaviorType) => 
        behaviorType.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .Any(t => t.GetGenericTypeDefinition() == _handlerBehaviorAbstractionType);
    
}