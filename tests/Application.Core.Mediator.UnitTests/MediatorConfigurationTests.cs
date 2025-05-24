using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests;

public class MediatorConfigurationTests
{
    private static readonly Type[] _expectedHandlerTypesToRegister =
    [
        typeof(IHandler<TestLogRequest, string>),
        typeof(IHandler<TestRequest, int>)
    ];

    private readonly MediatorConfiguration _mediatorConfiguration = new();

    [Fact]
    public void AddBehavior_RegistersBehavior_WhenRegisteringOneBehavior()
    {
        // Act
        _mediatorConfiguration.AddBehavior(typeof(TestBehavior<,>));

        // Assert
        Assert.Contains(_mediatorConfiguration.ServicesToRegister, service => service.ServiceType == typeof(IBehavior<,>) &&
                                                                              service.ImplementationType == typeof(TestBehavior<,>));
    }

    [Fact]
    public void AddBehavior_RegistersBehaviors_WhenRegisteringMultipleBehaviors()
    {
        Type[] expectedBehaviorTypes =
        [
            typeof(TestBehavior<,>),
            typeof(AnotherTestBehavior<,>)
        ];
        // Act
        _mediatorConfiguration.AddBehavior(typeof(TestBehavior<,>))
                              .AddBehavior(typeof(AnotherTestBehavior<,>));

        // Assert
        Assert.All(expectedBehaviorTypes, expectedBehaviorType =>
            Assert.Single(_mediatorConfiguration.ServicesToRegister, service => service.ServiceType == typeof(IBehavior<,>) &&
                                                                                service.ImplementationType == expectedBehaviorType));
    }

    [Fact]
    public void AddBehavior_ThrowsArgumentException_WhenSpecifiedTypeIsNotABehavior()
    {
        // Act
        // ReSharper disable once ConvertToLocalFunction
        var action = () => _mediatorConfiguration.AddBehavior(typeof(TestLogRequestHandler));

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void AddHandlersFromAssemblyContainingType_RegistersTheExpectedHandlers()
    {
        // Arrange
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        
        // Act
        _mediatorConfiguration.AddHandlersFromAssemblyContaining<TestLogRequest>(lifetime);

        // Assert
        Assert.All(_expectedHandlerTypesToRegister, expectedType =>
            Assert.Single(_mediatorConfiguration.ServicesToRegister, service => service.ServiceType == expectedType &&
                                                                                service.Lifetime == lifetime));
    }

    [Fact]
    public void AddHandlersFromAssemblies_RegistersTheExpectedHandlers()
    {
        // Arrange
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        
        // Act
        _mediatorConfiguration.AddHandlersFromAssemblies([typeof(TestLogRequest).Assembly], lifetime);

        // Assert
        Assert.All(_expectedHandlerTypesToRegister, expectedType =>
            Assert.Single(_mediatorConfiguration.ServicesToRegister, service => service.ServiceType == expectedType &&
                                                                                service.Lifetime == lifetime));
    }

    [Fact]
    public void AddHandlersFromAssemblies_ThrowsArgumentException_WhenNoAssemblyIsSpecified()
    {
        // Act
        // ReSharper disable once ConvertToLocalFunction
        var action = () => _mediatorConfiguration.AddHandlersFromAssemblies([]);

        Assert.Throws<ArgumentException>(action);
    }
}