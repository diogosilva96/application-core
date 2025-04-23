using Application.Core.Mediator.IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.IntegrationTests;

public class SenderTests
{
    [Fact]
    public async Task SendAsync_ExecutesSuccessfully_WhenHandlerIsConfigured()
    {
        // Arrange
        var factory = new IntegrationTestFactory()
            .ConfigureServices(services => services.AddMediator(config => config.AddHandlersFromAssemblyContaining<TestRequest>()));
        factory.Build();
        var sender = factory.CreateSender();
        var request = new TestRequest()
        {
            Value = 5
        };

        // Act
        var result = await sender.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(request.Value, result);
    }
    
    [Fact]
    public async Task SendAsync_ExecutesSuccessfullyAndInTheCorrectOrder_WhenHandlerAndBehaviorsAreConfigured()
    {
        // Arrange
        var factory = new IntegrationTestFactory()
            .ConfigureServices(services =>
            {
                services.AddMediator(config => config.AddHandlersFromAssemblyContaining<TestRequest>()
                                                     // behaviors are registered as singleton to ensure that they are called
                                                     .AddBehavior(typeof(TestBehavior<,>), ServiceLifetime.Singleton)
                                                     .AddBehavior(typeof(AnotherTestBehavior<,>), ServiceLifetime.Singleton));
            });
        factory.Build();
        var sender = factory.CreateSender();
        var testBehavior = factory.ServiceProvider.GetServices<IBehavior<TestRequest, int>>().OfType<TestBehavior<TestRequest, int>>().Single();
        var anotherTestBehavior = factory.ServiceProvider.GetServices<IBehavior<TestRequest, int>>().OfType<AnotherTestBehavior<TestRequest, int>>().Single();
        var request = new TestRequest
        {
            Value = 10
        };

        // Act
        var result = await sender.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(request.Value, result);
        Assert.Equal(1, testBehavior.HandledCount);
        Assert.Equal(1, anotherTestBehavior.HandledCount);
        Assert.True(anotherTestBehavior.HandledAtTicks > testBehavior.HandledAtTicks);
    }
}