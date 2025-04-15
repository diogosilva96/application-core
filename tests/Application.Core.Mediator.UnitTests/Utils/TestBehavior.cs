namespace Application.Core.Mediator.UnitTests.Utils;

internal class TestBehavior<TRequest, TResponse> : IHandlerBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        TestContext.Current.TestOutputHelper?.WriteLine("Test Behavior");

        return next();
    }
}