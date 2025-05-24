using Application.Core.Mediator.UnitTests.Utils;
using AutoFixture;
using NSubstitute;

namespace Application.Core.Mediator.UnitTests;

public class RequestPipelineTests
{
    private readonly Fixture _fixture = new();
    private readonly IHandler<TestRequest, int> _handler = Substitute.For<IHandler<TestRequest, int>>();

    [Fact]
    public async Task ExecuteAsync_ExecutesAllBehaviorsAndHandler_WhenBehaviorsCallNextDelegate()
    {
        // Arrange
        var expectedResponse = _fixture.Create<int>();
        var request = new TestRequest();

        var testBehavior = new TestBehavior<TestRequest, int>();
        var anotherTestBehavior = new AnotherTestBehavior<TestRequest, int>();
        _handler.HandleAsync(request, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(expectedResponse);

        var pipeline = new RequestPipeline<TestRequest, int>([testBehavior, anotherTestBehavior], _handler);

        // Act
        var result = await pipeline.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse, result);
        Assert.True(testBehavior.HandledAtTicks < anotherTestBehavior.HandledAtTicks);
        Assert.Equal(1, testBehavior.HandledCount);
        Assert.Equal(1, anotherTestBehavior.HandledCount);
        await _handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ExecutesUpToBehaviorThatReturnsAResult_WhenABehaviorReturnsResult()
    {
        // Arrange
        var expectedResponse = _fixture.Create<int>();
        var request = new TestRequest();

        var testBehavior = new TestBehavior<TestRequest, int>();
        // Execution should stop on aBehavior as it returns a result
        var aBehavior = Substitute.For<IBehavior<TestRequest, int>>();
        aBehavior.HandleAsync(Arg.Any<TestRequest>(), Arg.Any<Func<Task<int>>>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(expectedResponse);
        var anotherBehavior = Substitute.For<IBehavior<TestRequest, int>>();

        var pipeline = new RequestPipeline<TestRequest, int>([testBehavior, aBehavior, anotherBehavior], _handler);

        // Act
        var result = await pipeline.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse, result);
        Assert.Equal(1, testBehavior.HandledCount);
        await aBehavior.Received(1).HandleAsync(request, Arg.Any<Func<Task<int>>>(), Arg.Any<CancellationToken>());
        await anotherBehavior.DidNotReceiveWithAnyArgs().HandleAsync(Arg.Any<TestRequest>(), Arg.Any<Func<Task<int>>>(), Arg.Any<CancellationToken>());
        await _handler.DidNotReceiveWithAnyArgs().HandleAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ExecutesHandlerDirectly_WhenNoBehaviorExists()
    {
        // Arrange
        var expectedResponse = _fixture.Create<int>();
        var request = new TestRequest();

        _handler.HandleAsync(request, Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var pipeline = new RequestPipeline<TestRequest, int>([], _handler);

        // Act
        var result = await pipeline.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse, result);
        await _handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
    }
}