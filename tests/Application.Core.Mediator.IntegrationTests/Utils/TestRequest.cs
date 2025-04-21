namespace Application.Core.Mediator.IntegrationTests.Utils;

internal record TestRequest : IRequest<int>
{
    public required int Value { get; init; }
}