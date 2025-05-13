using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Application.Core.Api.UnitTests.Builder;

public class HttpContextBuilder
{
    private IEndpointFeature? _endpointFeature;
    private IServiceProvider? _requestServices;

    public HttpContext Build()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices =  _requestServices ?? new ServiceCollection().BuildServiceProvider()
        };
        
        if (_endpointFeature is not null)
        {
            httpContext.Features.Set(_endpointFeature);
        }
        
        return httpContext;
    }

    public HttpContextBuilder WithRequestServices(IServiceProvider serviceProvider)
    {
        _requestServices = serviceProvider;

        return this;
    }
    
    public HttpContextBuilder WithEndpoint(object[] endpointMetadata)
    {
        var fixture = new Fixture();
        var endpointFeature = Substitute.For<IEndpointFeature>();
        var endpoint = new Endpoint(null, new(endpointMetadata), fixture.Create<string>());
        endpointFeature.Endpoint.Returns(endpoint);
        _endpointFeature = endpointFeature;

        return this;
    }
}