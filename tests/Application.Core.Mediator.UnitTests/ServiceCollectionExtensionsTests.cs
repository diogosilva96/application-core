using System.Collections.Concurrent;
using System.Reflection;
using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void AddMediator_RegistersExpectedServices()
    {
        // Arrange
        // Register handler to be able to ensure that the IRequestPipeline is registered
        _serviceCollection.AddTransient<IHandler<TestLogRequest, string>, TestLogRequestHandler>(); 
        
        // Act
        using var serviceProvider = _serviceCollection.AddMediator()
                                                      .BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IMediator>());
        Assert.NotNull(serviceProvider.GetKeyedService<ConcurrentDictionary<Type, MethodInfo>>(ServiceKeys.MediatorMethodCache));
        Assert.NotNull(serviceProvider.GetService<IRequestPipeline<TestLogRequest, string>>());
    }
    
    [Fact]
    public void AddMediator_RegistersExpectedTypes_WhenConfiguringMediatorConfiguration()
    {
        // Arrange
        Type[] expectedRegisteredTypes =
        [
            typeof(IMediator),
            typeof(IBehavior<TestLogRequest, string>),
            typeof(IHandler<TestLogRequest, string>),
            typeof(IHandler<TestRequest, int>),
            typeof(IRequestPipeline<TestLogRequest, string>)
        ];

        // Act
        var serviceProvider = _serviceCollection.AddMediator(configurator => configurator.AddBehavior(typeof(TestBehavior<,>))
                                                                                         .AddHandlersFromAssemblyContaining<TestLogRequest>())
                                                .BuildServiceProvider();

        // Assert
        Assert.All(expectedRegisteredTypes, expectedType => Assert.NotNull(serviceProvider.GetService(expectedType)));
    }
}