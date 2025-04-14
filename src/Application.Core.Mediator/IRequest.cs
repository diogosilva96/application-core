namespace Application.Core.Mediator;

/// <summary>
/// Represents a request with a response of type <see cref="T:TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequest<TResponse> : IBaseRequest;

/// <summary>
/// Marker interface for a base request.
/// </summary>
public interface IBaseRequest;