using Application.Core.Api.Result;
using AutoFixture;

namespace Application.Core.Api.UnitTests.Result;

public class ProblemDetailsTests
{
    private readonly Fixture _fixture = new();

    [Theory]
    [ClassData(typeof(ValidStatusCodeTestData))]
    public void Constructor_CreatesCorrectInstance_WhenNotSpecifyingOptionalProperties(int statusCode)
    {
        // Arrange
        var message = _fixture.Create<string>();

        // Act
        var result = new ProblemDetails(message, statusCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(message, result.Message);
        Assert.Equal(statusCode, result.Status);
        Assert.Null(result.Detail);
        Assert.Null(result.Instance);
        Assert.Null(result.Title);
        Assert.Null(result.Type);
        Assert.Empty(result.Extensions);
    }

    [Theory]
    [ClassData(typeof(ValidStatusCodeTestData))]
    public void Constructor_CreatesCorrectInstance_WhenSpecifyingOptionalProperties(int statusCode)
    {
        // Arrange
        var message = _fixture.Create<string>();
        var detail = _fixture.Create<string>();
        var instance = _fixture.Create<string>();
        var title = _fixture.Create<string>();
        var type = _fixture.Create<string>();
        var extensions = _fixture.Create<Dictionary<string, object?>>();

        // Act
        var result = new ProblemDetails(message, statusCode)
        {
            Detail = detail,
            Instance = instance,
            Title = title,
            Type = type,
            Extensions = extensions
        };

        // Assert
        Assert.NotNull(result);
        Assert.Equal(message, result.Message);
        Assert.Equal(statusCode, result.Status);
        Assert.Equal(detail, result.Detail);
        Assert.Equal(instance, result.Instance);
        Assert.Equal(title, result.Title);
        Assert.Equal(type, result.Type);
        Assert.Equal(extensions, result.Extensions);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(200)]
    [InlineData(399)]
    [InlineData(600)]
    [InlineData(6000)]
    public void Constructor_ThrowsArgumentOutOfRangeException_WhenSpecifyingInvalidErrorStatusCode(int statusCode)
    {
        // Arrange
        var message = _fixture.Create<string>();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new ProblemDetails(message, statusCode));
    }

    private class ValidStatusCodeTestData : TheoryData<int>
    {
        public ValidStatusCodeTestData()
        {
            var statusCodes = Enumerable.Range(400, 199).Select(statusCode => statusCode);
            AddRange(statusCodes);
        }
    }
}