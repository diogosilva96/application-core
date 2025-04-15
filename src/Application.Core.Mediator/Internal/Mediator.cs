using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.Internal;

/// <summary>
/// Represents the mediator component.
/// </summary>
internal class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private readonly ConcurrentDictionary<Type, MethodInfo> _methodCache = [];
    
    /// <inheritdoc />
    public Task<TResponse> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var handlerBehaviorType = typeof(IHandlerBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handlerBehaviors = serviceProvider.GetServices(handlerBehaviorType).OfType<object>().ToArray();
        
        return HandleInternalAsync(handlerBehaviors, 0);
        
        Task<TResponse> HandleInternalAsync(object[] behaviors, int index)
        {
            var behaviorsSlice = behaviors.AsSpan()[index..];
            if (behaviorsSlice.Length == 0) return HandleUsingHandlerAsync(request, cancellationToken);

            if (!_methodCache.TryGetValue(handlerBehaviorType, out var behaviorMethod))
            {
                behaviorMethod = handlerBehaviorType.GetMethod(nameof(IHandlerBehavior<IRequest<TResponse>, TResponse>.HandleAsync))!;
                _methodCache.TryAdd(handlerBehaviorType, behaviorMethod);
            }
            
            var currentBehavior = behaviorsSlice[0];
            return (Task<TResponse>)behaviorMethod.Invoke(currentBehavior, [request, () => HandleInternalAsync(behaviors, index + 1), cancellationToken])!;
        }
    }

    private Task<TResponse> HandleUsingHandlerAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var handlerType = typeof(IHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"Could not find handler for request of type {request.GetType()} and response of type {typeof(TResponse).Name}.");
        }
        
        // ReSharper disable once InvertIf
        if (!_methodCache.TryGetValue(handlerType, out var handlerMethod))
        {
            handlerMethod = handlerType.GetMethod(nameof(IHandler<IRequest<TResponse>, TResponse>.HandleAsync))!;
            _methodCache.TryAdd(handlerType, handlerMethod);
        }
        
        return (Task<TResponse>)handlerMethod.Invoke(handler, [request, cancellationToken ])!;
    }
}