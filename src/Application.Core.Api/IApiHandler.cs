using Application.Core.Api.Result;
using Application.Core.Mediator;

namespace Application.Core.Api;

/// <summary>
/// Represents a handler for processing requests that return results of type <see cref="ApiResult"/>.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
public interface IApiHandler<in TRequest> : IHandler<TRequest, ApiResult> where TRequest : IRequest<ApiResult>;