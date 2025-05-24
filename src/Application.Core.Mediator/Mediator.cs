using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator;

/// <summary>
/// Represents the mediator component.
/// </summary>
internal class Mediator(IServiceProvider serviceProvider, [FromKeyedServices(ServiceKeys.MediatorMethodCache)]ConcurrentDictionary<Type, MethodInfo> methodCache) : IMediator
{
    /// <inheritdoc />
    public Task<TResponse> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestPipelineType = typeof(IRequestPipeline<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var requestPipeline = serviceProvider.GetRequiredService(requestPipelineType);
       
        // ReSharper disable once InvertIf
        if (!methodCache.TryGetValue(requestPipelineType, out var executeMethod))
        {
            executeMethod = requestPipelineType.GetMethod(nameof(IRequestPipeline<IRequest<TResponse>, TResponse>.ExecuteAsync))!;
            methodCache.TryAdd(requestPipelineType, executeMethod);
        }

        return (Task<TResponse>)(executeMethod.Invoke(requestPipeline, [request, cancellationToken]) ??
                                 throw new InvalidOperationException($"Request pipeline '{requestPipeline.GetType().Name}' for request of type '{request.GetType().Name}' and response of type '{typeof(TResponse).Name}' should not return null when handling the request."));
    }
}