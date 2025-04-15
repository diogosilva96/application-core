namespace Application.Core.Mediator.UnitTests.Utils;

internal record TestLogRequest : IRequest<string>
{
    public required Guid Id { get; init; }

    public required string Message { get; init; }
}