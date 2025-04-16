using Microsoft.Extensions.Logging;

namespace Application.Core.Mediator.Sample;

public class PingHandler(TimeProvider timeProvider, ILogger<PingHandler> logger) : IHandler<Ping, Pong>
{
    public async Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Handling {Ping}", request);
        // Delay to simulate work
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(50, 1000)), cancellationToken);
        return new Pong
        {
            TimestampUtc = timeProvider.GetUtcNow()
        };
    }
}