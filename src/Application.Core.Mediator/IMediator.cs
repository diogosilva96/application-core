﻿namespace Application.Core.Mediator;

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
    /// <exception cref="InvalidOperationException">
    /// Exception thrown when no handler can be found for the given request and response types.
    /// </exception>
    public Task<TResponse> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <exception cref="InvalidOperationException">
    /// Exception thrown when no handler can be found for the given request type.
    /// </exception>
    public Task HandleAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
}