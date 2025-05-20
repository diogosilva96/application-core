using System.Collections.Concurrent;
using System.Reflection;
using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void AddMediator_RegistersMediator()
    {
        // Act
        using var serviceProvider = _serviceCollection.AddMediator()
                                                      .BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IMediator>());
    }

    [Fact]
    public void AddMediator_RegistersMediatorMethodCache()
    {
        // Act
        using var serviceProvider = _serviceCollection.AddMediator()
                                                      .BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetKeyedService<ConcurrentDictionary<Type, MethodInfo>>(ServiceKeys.MediatorMethodCache));
    }

    [Fact]
    public void AddMediator_RegistersExpectedTypes_WhenUsingMediatorConfiguratorAction()
    {
        // Arrange
        Type[] expectedTypesToRegister =
        [
            typeof(IMediator),
            typeof(IBehavior<TestLogRequest, string>),
            typeof(IHandler<TestLogRequest, string>),
            typeof(IHandler<TestRequest, int>)
        ];

        // Act
        var serviceProvider = _serviceCollection.AddMediator(configurator => configurator.AddBehavior(typeof(TestBehavior<,>))
                                                                                         .AddHandlersFromAssemblyContaining<TestLogRequest>())
                                                .BuildServiceProvider();

        // Assert
        Assert.All(expectedTypesToRegister, expectedType => Assert.NotNull(serviceProvider.GetService(expectedType)));
    }
}