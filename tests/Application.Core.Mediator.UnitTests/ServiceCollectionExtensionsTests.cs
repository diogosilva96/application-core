using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Mediator.UnitTests;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void AddMediator_RegistersMediator()
    {
        // Act
        _serviceCollection.AddMediator();
        using var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IMediator>());
    }
}