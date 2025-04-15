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

    private readonly MediatorConfigurator _mediatorConfigurator;
    private readonly ServiceCollection _serviceCollection;

    public MediatorConfiguratorTests()
    {
        _serviceCollection = [];
        _mediatorConfigurator = new(_serviceCollection);
    }

    [Fact]
    public void AddBehavior_RegistersBehavior()
    {
        // Act
        _mediatorConfigurator.AddBehavior(typeof(TestBehavior<,>));
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IHandlerBehavior<TestLogRequest, string>>());
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
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Assert
        Assert.All(_expectedHandlerTypesToRegister, expectedType => Assert.NotNull(serviceProvider.GetService(expectedType)));
    }

    [Fact]
    public void AddHandlersFromAssemblies_RegistersTheExpectedHandlers()
    {
        // Act
        _mediatorConfigurator.AddHandlersFromAssemblies(typeof(TestLogRequest).Assembly);
        var serviceProvider = _serviceCollection.BuildServiceProvider();

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