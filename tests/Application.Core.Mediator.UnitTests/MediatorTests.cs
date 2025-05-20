using Application.Core.Mediator.UnitTests.Builder;
using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Application.Core.Mediator.UnitTests;

public class MediatorTests
{
    [Fact]
    public async Task HandleAsync_ReturnsResponse_WhenHandlerExists()
    {
        // Arrange
        const string expectedResult = "test-response";

        var handler = Substitute.For<IHandler<TestLogRequest, string>>();
        handler.HandleAsync(Arg.Any<TestLogRequest>(), Arg.Any<CancellationToken>()).Returns(expectedResult);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => handler);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var mediator = new MediatorBuilder().With(serviceProvider).Build();

        // Act
        var result = await mediator.HandleAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(expectedResult, result);
        await handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ReturnsResponseAndExecutesInExpectedOrder_WhenHandlerAndBehaviorsExist()
    {
        // Arrange
        const string expectedResult = "test-response";
        var handler = Substitute.For<IHandler<TestLogRequest, string>>();
        handler.HandleAsync(Arg.Any<TestLogRequest>(), Arg.Any<CancellationToken>()).Returns(expectedResult);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IHandler<TestLogRequest, string>>(_ => handler)
                         // the behaviors are setup as a singleton so that we can check that they have been handled
                         .AddSingleton(typeof(IBehavior<,>), typeof(TestBehavior<,>))
                         .AddSingleton(typeof(IBehavior<,>), typeof(AnotherTestBehavior<,>));
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var behaviors = serviceProvider.GetServices<IBehavior<TestLogRequest, string>>().ToArray();
        // ReSharper disable twice SuspiciousTypeConversion.Global
        var testBehavior = behaviors.OfType<TestBehavior<TestLogRequest, string>>().Single();
        var anotherTestBehavior = behaviors.OfType<AnotherTestBehavior<TestLogRequest, string>>().Single();

        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var mediator = new MediatorBuilder().With(serviceProvider)
                                            .Build();

        // Act
        var result = await mediator.HandleAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(1, anotherTestBehavior.HandledCount);
        Assert.Equal(1, testBehavior.HandledCount);
        Assert.True(anotherTestBehavior.HandledAtTicks > testBehavior.HandledAtTicks);
        await handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
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