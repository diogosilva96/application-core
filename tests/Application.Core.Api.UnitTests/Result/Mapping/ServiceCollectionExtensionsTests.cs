using Application.Core.Api.Result.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Api.UnitTests.Result.Mapping;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _serviceCollection;

    public ServiceCollectionExtensionsTests()
    {
        _serviceCollection = [];
        _serviceCollection.AddLogging();
    }

    [Fact]
    public void AddApiResultMapping_RegistersExpectedServices()
    {
        // Act
        using var serviceProvider = _serviceCollection.AddApiResultMapping()
                                                      .BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IApiResultMapper>());
        Assert.NotNull(serviceProvider.GetService<IHttpContextAccessor>());
    }
}