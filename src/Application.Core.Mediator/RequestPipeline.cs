namespace Application.Core.Mediator;

/// <summary>
/// Represents a request pipeline component.
/// </summary>
/// <param name="behaviors">The behaviors to use.</param>
/// <param name="handler">The handler to use.</param>
/// <typeparam name="TRequest">The type of the request. Must implement <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
internal class RequestPipeline<TRequest, TResponse>(
    IEnumerable<IBehavior<TRequest, TResponse>> behaviors, 
    IHandler<TRequest, TResponse> handler) 
    : IRequestPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IBehavior<TRequest, TResponse>[] _behaviors = behaviors.ToArray();
    
    /// <inheritdoc />
    public Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return ExecuteInternalAsync(0);
        
        Task<TResponse> ExecuteInternalAsync(int index)
        {
            var behaviorsSlice = _behaviors.AsSpan()[index..];
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (behaviorsSlice.Length == 0)
            {
                return handler.HandleAsync(request, cancellationToken);
            }

            return behaviorsSlice[0].HandleAsync(request, () => ExecuteInternalAsync(index + 1), cancellationToken);
        }
    }
}