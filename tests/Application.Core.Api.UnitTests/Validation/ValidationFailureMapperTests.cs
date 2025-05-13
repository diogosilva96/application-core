using Application.Core.Api.UnitTests.Builder;
using Application.Core.Api.UnitTests.Utils;
using Application.Core.Api.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Application.Core.Api.UnitTests.Validation;

public class ValidationFailureMapperTests
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ValidationFailureMapper _mapper;

    public ValidationFailureMapperTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _mapper = new(_httpContextAccessor, Substitute.For<ILogger<ValidationFailureMapper>>());
    }

    [Fact]
    public void Map_ReturnsOriginalValidationFailures_WhenThereIsNoHttpContext()
    {
        // Arrange
        _httpContextAccessor.HttpContext.ReturnsNullForAnyArgs();
        var validationFailures = new Dictionary<string, string[]>
        {
            { "Field1", ["Error1", "Error2"] }
        };

        // Act
        var result = _mapper.Map(validationFailures);

        // Assert
        Assert.Equal(validationFailures, result);
    }

    [Fact]
    public void Map_ReturnsOriginalValidationFailures_WhenPropertyMapperBindingCannotBeFound()
    {
        // Arrange
        var httpContext = new HttpContextBuilder().WithEndpoint([]).Build();
        _httpContextAccessor.HttpContext.Returns(httpContext);
        var validationFailures = new Dictionary<string, string[]>
        {
            { "Field1", ["Error1", "Error2"] }
        };

        // Act
        var result = _mapper.Map(validationFailures);

        // Assert
        Assert.Equal(validationFailures, result);
    }

    [Fact]
    public void Map_ThrowsInvalidOperationException_WhenPropertyMapperCannotBeFoundInDIContainer()
    {
        // Arrange
        var httpContext = new HttpContextBuilder()
                          .WithEndpoint([new ValidationFailurePropertyMapperBinding(typeof(TestRequestValidationFailurePropertyMapper))])
                          .WithRequestServices(new ServiceCollection().BuildServiceProvider())
                          .Build();
        _httpContextAccessor.HttpContext.Returns(httpContext);

        var validationFailures = new Dictionary<string, string[]>
        {
            { "Field1", ["Error1", "Error2"] }
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mapper.Map(validationFailures));
    }

    [Fact]
    public void Map_ReturnsMappedValidationFailures()
    {
        // Arrange
        var propertyMapperBinding = new ValidationFailurePropertyMapperBinding(typeof(TestRequestValidationFailurePropertyMapper));
        var serviceCollection = new ServiceCollection().AddSingleton<IValidationFailurePropertyMapper>(_ =>
        {
            var mapper = new TestRequestValidationFailurePropertyMapper();
            mapper.ConfigureMapping("Field1", "MappedField1");
            mapper.ConfigureMapping("Id", "Identifier");
            return mapper;
        });

        var httpContext = new HttpContextBuilder().WithEndpoint([propertyMapperBinding])
                                                  .WithRequestServices(serviceCollection.BuildServiceProvider())
                                                  .Build();
        _httpContextAccessor.HttpContext.Returns(httpContext);

        var validationFailures = new Dictionary<string, string[]>
        {
            { "Field1", ["Error1 with Field1", "Error2 with Field1"] },
            { "Id", ["Error1 with Id"] }
        };

        var expectedValidationFailures = new Dictionary<string, string[]>
        {
            { "MappedField1", ["Error1 with MappedField1", "Error2 with MappedField1"] },
            { "Identifier", ["Error1 with Identifier"] }
        };

        // Act
        var result = _mapper.Map(validationFailures);

        // Assert
        Assert.Equal(expectedValidationFailures, result);
    }
    
    [Fact]
    public void Map_ReturnsMappedValidationFailures_WhenPassingErrorMessageWithWordWithoutSpaces()
    {
        // Arrange
        var propertyMapperBinding = new ValidationFailurePropertyMapperBinding(typeof(TestRequestValidationFailurePropertyMapper));
        var serviceCollection = new ServiceCollection().AddSingleton<IValidationFailurePropertyMapper>(_ =>
        {
            var mapper = new TestRequestValidationFailurePropertyMapper();
            mapper.ConfigureMapping("Field1", "MappedField1");
            return mapper;
        });

        var httpContext = new HttpContextBuilder().WithEndpoint([propertyMapperBinding])
                                                  .WithRequestServices(serviceCollection.BuildServiceProvider())
                                                  .Build();
        _httpContextAccessor.HttpContext.Returns(httpContext);

        var validationFailures = new Dictionary<string, string[]>
        {
            { "Field1", ["Error1 withField1", "Error1 with Field123"] },
        };

        var expectedValidationFailures = new Dictionary<string, string[]>
        {
            { "MappedField1", ["Error1 withField1", "Error1 with Field123"] },
        };

        // Act
        var result = _mapper.Map(validationFailures);

        // Assert
        Assert.Equal(expectedValidationFailures, result);
    }
    
    [Fact]
    public void Map_ReturnsMappedValidationFailures_WhenPassingUnmappedProperty()
    {
        // Arrange
        var propertyMapperBinding = new ValidationFailurePropertyMapperBinding(typeof(TestRequestValidationFailurePropertyMapper));
        var serviceCollection = new ServiceCollection().AddSingleton<IValidationFailurePropertyMapper, TestRequestValidationFailurePropertyMapper>();

        var httpContext = new HttpContextBuilder().WithEndpoint([propertyMapperBinding])
                                                  .WithRequestServices(serviceCollection.BuildServiceProvider())
                                                  .Build();
        _httpContextAccessor.HttpContext.Returns(httpContext);

        var validationFailures = new Dictionary<string, string[]>
        {
            { "Test", ["Error1 with Test", "Error2 with Test"] },
            { "Test2", ["Error3 with Test2"] },
        };

        var expectedValidationFailures = new Dictionary<string, string[]>
        {
            { PropertyNames.Unknown, [$"Error1 with {PropertyNames.Unknown}", $"Error2 with {PropertyNames.Unknown}", $"Error3 with {PropertyNames.Unknown}"] }
        };

        // Act
        var result = _mapper.Map(validationFailures);

        // Assert
        Assert.Equal(validationFailures.Sum(x => x.Value.Length), result.Sum(x => x.Value.Length));
        Assert.Equivalent(expectedValidationFailures, result);
    }
}