using Application.Core.Api.Result;
using AutoFixture;
using FluentValidation.Results;

namespace Application.Core.Api.UnitTests.Result;

public class BadRequestTests
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public void Constructor_CreatesCorrectInstance()
    {
        // Arrange
        var message = _fixture.Create<string>();
        var failures = _fixture.CreateMany<ValidationFailure>().ToArray();

        // Act
        var result = new BadRequest(message, failures);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.Single(result.Extensions);
        Assert.Contains(result.Extensions, extension => extension.Key == "errors");
        Assert.Equal(result.Extensions["errors"], result.Errors);
        Assert.All(failures, failure => Assert.Contains(result.Errors, error => error.Key == failure.PropertyName && 
                                                                                error.Value.Contains(failure.ErrorMessage)));
    }
    
    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenOverridingExtensions()
    {
        // Arrange
        var message = _fixture.Create<string>();
        var failures = _fixture.CreateMany<ValidationFailure>().ToArray();
        var (customKey, customValue) = ("custom-key", "custom-value" as object);

        // Act
        var result = new BadRequest(message, failures)
        {
            Extensions = new Dictionary<string, object?>()
            {
                { customKey, customValue }
            }
        };
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.Equal(2, result.Extensions.Count);
        Assert.Contains(result.Extensions, extension => extension.Key == customKey && extension.Value == customValue);
        Assert.Contains(result.Extensions, extension => extension.Key == "errors");
        Assert.All(failures, failure => Assert.Contains(result.Errors, error => error.Key == failure.PropertyName && 
                                                                                error.Value.Contains(failure.ErrorMessage)));
    }
}