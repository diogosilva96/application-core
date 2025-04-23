namespace Application.Core.Mediator.UnitTests.Utils;

internal class AnotherTestBehavior<TRequest, TResponse> : IBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public long HandledAtTicks { get; set; }
    
    public int HandledCount { get; set; }
    
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        TestContext.Current.TestOutputHelper?.WriteLine($"Another test Behavior for request {request}");
        HandledAtTicks = DateTimeOffset.UtcNow.Ticks;
        HandledCount++;
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        return await next();
    }
}