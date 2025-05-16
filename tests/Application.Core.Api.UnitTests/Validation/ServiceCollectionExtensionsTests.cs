using Application.Core.Api.UnitTests.Utils;
using Application.Core.Api.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Api.UnitTests.Validation;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _serviceCollection;

    public ServiceCollectionExtensionsTests()
    {
        _serviceCollection = [];
        _serviceCollection.AddLogging();
    }

    [Fact]
    public void AddValidationFailureMapping_ShouldRegisterValidationFailureMapper()
    {
        // Act
        _serviceCollection.AddValidationFailureMapping();
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IValidationFailureMapper>());
    }

    [Fact]
    public void AddValidationFailureMapping_ShouldRegisterHttpContextAccessor()
    {
        // Act
        _serviceCollection.AddValidationFailureMapping();
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IHttpContextAccessor>());
    }

    [Fact]
    public void AddValidationFailureMapping_ShouldRegisterMappersFromAssemblies()
    {
        // Act
        _serviceCollection.AddValidationFailureMapping(typeof(TestValidationFailurePropertyMapper).Assembly);
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        // Assert
        var mappers = serviceProvider.GetServices<IValidationFailurePropertyMapper>().ToArray();
        Assert.NotEmpty(mappers);
        Assert.Contains(mappers, mapper => mapper.GetType() == typeof(TestValidationFailurePropertyMapper));
    }
}