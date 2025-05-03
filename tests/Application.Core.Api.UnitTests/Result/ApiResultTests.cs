using Application.Core.Api.Result;
using AutoFixture;

namespace Application.Core.Api.UnitTests.Result;

public class ApiResultTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_CreatesSuccessfulResult_WhenASuccessIsSpecified()
    {
        // Arrange
        var value = new Ok(_fixture.Create<object>());

        // Act
        var result = new ApiResult(value);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Constructor_CreatesSuccessfulResultUsingImplicitConversion_WhenASuccessIsSpecified()
    {
        // Arrange
        var value = new Ok(_fixture.Create<object>());

        // Act
        ApiResult result = value;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Constructor_CreatesErrorResult_WhenAProblemDetailsIsSpecified()
    {
        // Arrange
        var error = new NotFound(_fixture.Create<string>());

        // Act
        var result = new ApiResult(error);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Constructor_CreatesErrorResultUsingImplicitConversion_WhenAProblemDetailsIsSpecified()
    {
        // Arrange
        var error = new NotFound(_fixture.Create<string>());

        // Act
        ApiResult result = error;

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
}