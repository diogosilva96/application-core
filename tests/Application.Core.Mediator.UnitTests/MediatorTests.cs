using Application.Core.Mediator.UnitTests.Builder;
using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Application.Core.Mediator.UnitTests;

public class MediatorTests
{
    [Fact]
    public async Task HandleAsync_ReturnsExpectedResponse_WhenHandlerExists()
    {
        // Arrange
        const string expectedResult = "test-response";

        var requestPipeline = Substitute.For<IRequestPipeline<TestLogRequest, string>>();
        requestPipeline.ExecuteAsync(Arg.Any<TestLogRequest>(), Arg.Any<CancellationToken>()).Returns(expectedResult);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => requestPipeline);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var mediator = new MediatorBuilder().With(serviceProvider).Build();

        // Act
        var result = await mediator.HandleAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(expectedResult, result);
        await requestPipeline.Received(1).ExecuteAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ThrowsInvalidOperationException_WhenHandlerCannotBeResolved()
    {
        // Arrange
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var mediator = new MediatorBuilder().With(serviceProvider)
                                            .Build();

        // Act
        // ReSharper disable once ConvertToLocalFunction
        var action = () => mediator.HandleAsync(request, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }
}