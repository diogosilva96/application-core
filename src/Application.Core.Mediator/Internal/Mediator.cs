using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.Internal;

/// <summary>
/// Represents the mediator component.
/// </summary>
internal class Mediator(IServiceProvider serviceProvider) : IMediator
{
    /// <inheritdoc />
    public Task<TResponse> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var handlerType = typeof(IHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"Could not find handler for request of type {request.GetType()} and response of type {typeof(TResponse).Name}.");
        }
        
        var handlerMethod = handlerType.GetMethod(nameof(IHandler<IRequest<TResponse>, TResponse>.HandleAsync)) ?? 
                            throw new InvalidOperationException($"Could not find handler method '{nameof(IHandler<IRequest<TResponse>, TResponse>.HandleAsync)}' for handler '{handlerType.Name}'.");
        
        return (Task<TResponse>)handlerMethod.Invoke(handler, [request, cancellationToken ])!;
    }
}