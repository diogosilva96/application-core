using Application.Core.Api.Result;
using Application.Core.Mediator;

namespace Application.Core.Api.UnitTests.Utils;

public record TestRequest : IRequest<ApiResult>
{
    public Guid Id { get; init; }
}