using Application.Core.Api.RequestProcessing;
using Application.Core.Api.Result.Mapping;
using Application.Core.Mediator;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Application.Core.Api.UnitTests.RequestProcessing;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _serviceCollection;

    public ServiceCollectionExtensionsTests()
    {
        _serviceCollection = [];
        _serviceCollection.AddLogging()
                          .AddSingleton(Substitute.For<IMediator>())
                          .AddSingleton(Substitute.For<IApiResultMapper>())
                          .AddHttpContextAccessor();
    }

    [Fact]
    public void AddApiRequestProcessor_RegistersExpectedServices()
    {
        // Act
        using var serviceProvider = _serviceCollection.AddApiRequestProcessor()
                                                      .BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IApiRequestProcessor>());
    }
}