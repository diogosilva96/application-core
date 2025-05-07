using Application.Core.Api.Result;
using Application.Core.Mediator;
using Microsoft.AspNetCore.Http;

namespace Application.Core.Api.RequestProcessing;

/// <summary>
/// Represents an abstraction for a component responsible for processing API requests.
/// </summary>
public interface IApiRequestProcessor
{
    /// <summary>
    /// Processes the given <paramref name="request" /> asynchronously and returns an <see cref="IResult" /> representing its
    /// outcome.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <returns>An <see cref="IResult" /> representing the outcome of the request processing.</returns>
    public Task<IResult> ProcessAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<ApiResult>;
}