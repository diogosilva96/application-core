namespace Application.Core.Mediator;

/// <summary>
/// Represents a handler for a given request and response type.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified <paramref name="request"/> asynchronously.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resulting <see cref="TResponse"/>.</returns>
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}