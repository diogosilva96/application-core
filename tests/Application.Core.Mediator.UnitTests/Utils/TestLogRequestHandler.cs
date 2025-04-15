namespace Application.Core.Mediator.UnitTests.Utils;

internal class TestLogRequestHandler : IHandler<TestLogRequest, string>
{
    public Task<string> HandleAsync(TestLogRequest request, CancellationToken cancellationToken = default)
    {
        TestContext.Current.TestOutputHelper?.WriteLine($"Handling {request}");

        return Task.FromResult("Test");
    }
}