namespace Application.Core.Mediator;

/// <summary>
/// Represents a mediator component abstraction.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Handles the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>The resulting response of type <see cref="T:TResponse"/>.</returns>
    public Task<TResponse> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Handles the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public Task HandleAsync(IRequest request, CancellationToken cancellationToken = default);
}