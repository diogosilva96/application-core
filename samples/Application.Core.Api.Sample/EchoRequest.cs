using Application.Core.Api.Result;
using Application.Core.Mediator;

namespace Application.Core.Api.Sample;

public record EchoRequest(string Message) : IRequest<ApiResult>;