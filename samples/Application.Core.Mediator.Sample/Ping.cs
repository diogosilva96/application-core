namespace Application.Core.Mediator.Sample;

public record Ping : IRequest<Pong>
{
    public required DateTimeOffset TimestampUtc { get; init; }
}