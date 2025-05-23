namespace Application.Core.Mediator;

/// <summary>
/// Represents a request pipeline abstraction.
/// </summary>
/// <typeparam name="TRequest">The type of the request. Must implement <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestPipeline<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Executes the processing pipeline for the given request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request object to be processed by the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token used to observe cancellation requests.</param>
    /// <returns>The resulting response.<typeparamref name="TResponse"/>.</returns>
    public Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}