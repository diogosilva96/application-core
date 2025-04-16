using Application.Core.Mediator.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests;

public class MediatorConfiguratorTests
{
    private static readonly Type[] _expectedHandlerTypesToRegister =
    [
        typeof(IHandler<TestLogRequest, string>),
        typeof(IHandler<TestRequest, int>)
    ];

    private readonly MediatorConfigurator _mediatorConfigurator = new();
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void ConfigureServices_RegistersSender()
    {
        // Act
        var serviceProvider = _mediatorConfigurator.ConfigureServices(_serviceCollection).BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<ISender>());
    }

    [Fact]
    public void AddBehavior_RegistersBehavior_WhenRegisteringOneBehavior()
    {
        // Act
        _mediatorConfigurator.AddBehavior(typeof(TestBehavior<,>));
        var serviceProvider = _mediatorConfigurator.ConfigureServices(_serviceCollection).BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IHandlerBehavior<TestLogRequest, string>>());
    }
    
    [Fact]
    public void AddBehavior_RegistersBehaviors_WhenRegisteringMultipleBehaviors()
    {
        Type[] expectedBehaviorTypes =
        [
            typeof(TestBehavior<TestLogRequest, string>),
            typeof(AnotherTestBehavior<TestLogRequest, string>)
        ];
        // Act
        _mediatorConfigurator.AddBehavior(typeof(TestBehavior<,>))
                             .AddBehavior(typeof(AnotherTestBehavior<,>));
        var serviceProvider = _mediatorConfigurator.ConfigureServices(_serviceCollection).BuildServiceProvider();
        var behaviors = serviceProvider.GetServices<IHandlerBehavior<TestLogRequest, string>>().ToArray();

        // Assert
        Assert.All(expectedBehaviorTypes, expectedBehaviorType => Assert.Single(behaviors, behavior => behavior.GetType() == expectedBehaviorType));
    }

    [Fact]
    public void AddBehavior_ThrowsArgumentException_WhenSpecifiedTypeIsNotAHandlerBehavior()
    {
        // Act
        // ReSharper disable once ConvertToLocalFunction
        var action = () => _mediatorConfigurator.AddBehavior(typeof(TestLogRequestHandler));

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void AddHandlersFromAssemblyContainingType_RegistersTheExpectedHandlers()
    {
        // Act
        _mediatorConfigurator.AddHandlersFromAssemblyContaining<TestLogRequest>();
        var serviceProvider = _mediatorConfigurator.ConfigureServices(_serviceCollection).BuildServiceProvider();

        // Assert
        Assert.All(_expectedHandlerTypesToRegister, expectedType => Assert.NotNull(serviceProvider.GetService(expectedType)));
    }

    [Fact]
    public void AddHandlersFromAssemblies_RegistersTheExpectedHandlers()
    {
        // Act
        _mediatorConfigurator.AddHandlersFromAssemblies(typeof(TestLogRequest).Assembly);
        var serviceProvider = _mediatorConfigurator.ConfigureServices(_serviceCollection).BuildServiceProvider();

        // Assert
        Assert.All(_expectedHandlerTypesToRegister, expectedType => Assert.NotNull(serviceProvider.GetService(expectedType)));
    }

    [Fact]
    public void AddHandlersFromAssemblies_ThrowsArgumentException_WhenNoAssemblyIsSpecified()
    {
        // Act
        // ReSharper disable once ConvertToLocalFunction
        var action = () => _mediatorConfigurator.AddHandlersFromAssemblies();

        Assert.Throws<ArgumentException>(action);
    }
}