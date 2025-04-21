using Application.Core.Mediator.Internal;
using Application.Core.Mediator.UnitTests.Internal.Builder;
using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Application.Core.Mediator.UnitTests.Internal;

public class SenderTests
{
    [Fact]
    public async Task SendAsync_ReturnsResponse_WhenHandlerExists()
    {
        // Arrange
        const string expectedResult = "test-response"; 
        
        var handler = Substitute.For<IHandler<TestLogRequest, string>>();
        handler.HandleAsync(Arg.Any<TestLogRequest>(), Arg.Any<CancellationToken>()).Returns(expectedResult);
       
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => handler);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var sender = new SenderBuilder().With(serviceProvider)
                                        .Build();
        
        // Act
        var result = await sender.SendAsync(request, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.Equivalent(expectedResult, result);
        await handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task SendAsync_ReturnsResponseAndExecutesInExpectedOrder_WhenHandlerAndBehaviorsExist()
    {
        // Arrange
        const string expectedResult = "test-response"; 
        var handler = Substitute.For<IHandler<TestLogRequest, string>>();
        handler.HandleAsync(Arg.Any<TestLogRequest>(), Arg.Any<CancellationToken>()).Returns(expectedResult);
       
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IHandler<TestLogRequest, string>>(_ => handler)
                         // the behaviors are setup as a singleton so that we can check that they have been handled
                         .AddSingleton(typeof(IHandlerBehavior<,>), typeof(TestBehavior<,>))
                         .AddSingleton(typeof(IHandlerBehavior<,>), typeof(AnotherTestBehavior<,>));
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var behaviors = serviceProvider.GetServices<IHandlerBehavior<TestLogRequest, string>>().ToArray();
        // ReSharper disable twice SuspiciousTypeConversion.Global
        var testBehavior = behaviors.OfType<TestBehavior<TestLogRequest, string>>().Single();
        var anotherTestBehavior = behaviors.OfType<AnotherTestBehavior<TestLogRequest, string>>().Single();
        
        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var sender = new SenderBuilder().With(serviceProvider)
                                        .Build();
        
        // Act
        var result = await sender.SendAsync(request, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(1, anotherTestBehavior.HandledCount);
        Assert.Equal(1, testBehavior.HandledCount);
        Assert.True(anotherTestBehavior.HandledAtTicks > testBehavior.HandledAtTicks);
        await handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task SendAsync_ThrowsInvalidOperationException_WhenHandlerCannotBeResolved()
    {
        // Arrange
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        
        var request = new TestLogRequest { Id = Guid.NewGuid(), Message = "Hello this is a message." };
        var sender = new SenderBuilder().With(serviceProvider)
                                        .Build();
        
        // Act
        // ReSharper disable once ConvertToLocalFunction
        var action = () => sender.SendAsync(request, TestContext.Current.CancellationToken);
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }
}