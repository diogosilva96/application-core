using Application.Core.Api.UnitTests.Utils;
using Application.Core.Api.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Api.UnitTests.Validation;

public sealed class RouteHandlerBuilderExtensionsTests : IAsyncDisposable
{
    private readonly RouteHandlerBuilder _routeHandlerBuilder;
    private readonly WebApplication _webApplication;

    public RouteHandlerBuilderExtensionsTests()
    {
        var webApplicationBuilder = WebApplication.CreateSlimBuilder();
        var webApplication = webApplicationBuilder.Build();
        _webApplication = webApplication;
        _routeHandlerBuilder = webApplication.MapGet("/test", () => "test");
    }

    [Fact]
    public void WithValidationFailurePropertyMapper_ShouldRegisterValidationFailurePropertyMapperBindingMetadata()
    {
        // Act
        _routeHandlerBuilder.WithValidationFailurePropertyMapper<TestRequestValidationFailurePropertyMapper>();
        
        // we need to start the web application to be able to fetch the endpoint metadata
        _webApplication.StartAsync(TestContext.Current.CancellationToken);
        
        // Assert
        var endpoints = _webApplication.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        Assert.Contains(endpoints, endpoint => endpoint.Metadata.Any(x => x is ValidationFailurePropertyMapperBinding binding && 
                                                                          binding.MapperType == typeof(TestRequestValidationFailurePropertyMapper)));
    }

    public async ValueTask DisposeAsync()
    {
        await _webApplication.DisposeAsync();
    }
}