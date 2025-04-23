namespace Application.Core.Mediator;

/// <summary>
/// Represents a handler behavior for a given request and response type.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="next">The next delegate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resulting <see cref="TResponse"/>.</returns>
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default);
}
