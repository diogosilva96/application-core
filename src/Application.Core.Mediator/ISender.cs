namespace Application.Core.Mediator;

/// <summary>
/// Represents a sender component abstraction.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Sends the given <paramref name="request"/> for processing.
    /// </summary>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>The resulting response of type <see cref="T:TResponse"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Exception thrown when no underlying handler(s) can be found for the given request and response types.
    /// </exception>
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}