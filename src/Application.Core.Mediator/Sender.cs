using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Represents the sender component.
/// </summary>
internal class Sender(IServiceProvider serviceProvider, [FromKeyedServices(ServiceKeys.SenderMethodCache)]ConcurrentDictionary<Type, MethodInfo> methodCache) : ISender
{
    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var behaviorType = typeof(IBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var behaviors = serviceProvider.GetServices(behaviorType).OfType<object>().ToArray();
        
        return HandleInternalAsync(0);
        
        Task<TResponse> HandleInternalAsync(int index)
        {
            var behaviorsSlice = behaviors.AsSpan()[index..];
            if (behaviorsSlice.Length == 0) return HandleUsingHandlerAsync(request, cancellationToken);

            if (!methodCache.TryGetValue(behaviorType, out var behaviorMethod))
            {
                behaviorMethod = behaviorType.GetMethod(nameof(IBehavior<IRequest<TResponse>, TResponse>.HandleAsync))!;
                methodCache.TryAdd(behaviorType, behaviorMethod);
            }
            
            var currentBehavior = behaviorsSlice[0];
            return (Task<TResponse>)behaviorMethod.Invoke(currentBehavior, [request, () => HandleInternalAsync(index + 1), cancellationToken])!;
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
        if (!methodCache.TryGetValue(handlerType, out var handlerMethod))
        {
            handlerMethod = handlerType.GetMethod(nameof(IHandler<IRequest<TResponse>, TResponse>.HandleAsync))!;
            methodCache.TryAdd(handlerType, handlerMethod);
        }
        
        return (Task<TResponse>)handlerMethod.Invoke(handler, [request, cancellationToken ])!;
    }
}