namespace Application.Core.Mediator.UnitTests.Utils;

internal class TestRequestHandler : IHandler<TestRequest, int>
{
    public Task<int> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        TestContext.Current.TestOutputHelper?.WriteLine($"Handling {request}");

        return Task.FromResult(1);
    }
}