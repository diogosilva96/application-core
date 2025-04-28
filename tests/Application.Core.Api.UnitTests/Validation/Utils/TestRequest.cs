using Application.Core.Api.Result;
using Application.Core.Mediator;

namespace Application.Core.Api.UnitTests.Validation.Utils;

public record TestRequest : IRequest<ApiResult>
{ }