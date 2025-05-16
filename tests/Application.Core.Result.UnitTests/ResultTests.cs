using Application.Core.Result.UnitTests.Utils;

namespace Application.Core.Result.UnitTests;

public class ResultTests
{
    [Fact]
    public void Constructor_CreatesSuccessfulTypedErrorResultUsingImplicitConversion_WhenAValueIsSpecified()
    {
        // Arrange
        const string value = "test";

        // Act
        Result<string, TestError> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Constructor_CreatesErrorTypedErrorResultUsingImplicitConversion_WhenAnErrorIsSpecified()
    {
        // Arrange
        var error = new TestError("test error");

        // Act
        Result<string, TestError> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Constructor_CreatesSuccessfulTypedErrorResult_WhenAValueIsSpecified()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = new Result<string, TestError>(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullForTypedErrorResult_WhenNoValueIsSpecified()
    {
        // Arrange
        string value = null!;

        // Act & Assert 
        Assert.Throws<ArgumentNullException>(() => new Result<string, TestError>(value));
    }

    [Fact]
    public void Constructor_CreatesErrorTypedErrorResult_WhenAnErrorIsSpecified()
    {
        // Arrange
        var error = new TestError("test error");

        // Act
        var result = new Result<string, TestError>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullForErrorTypedResult_WhenNoErrorIsSpecified()
    {
        // Arrange
        TestError error = null!;

        // Act & Assert 
        Assert.Throws<ArgumentNullException>(() => new Result<string, TestError>(error));
    }

    [Fact]
    public void Match_ExecutesSuccessDelegate_WhenTheTypedErrorResultIsSuccessful()
    {
        // Arrange
        var result = new Result<int, TestError>(42);
        var successCalled = false;
        var errorCalled = false;
        const string successValue = "1";
        const string errorValue = "2";

        // Act
        var matchedResult = result.Match(
            _ =>
            {
                successCalled = true;
                return successValue;
            },
            _ =>
            {
                errorCalled = true;
                return errorValue;
            });

        // Assert
        Assert.True(successCalled);
        Assert.False(errorCalled);
        Assert.Equal(successValue, matchedResult);
    }

    [Fact]
    public void Match_ExecutesErrorDelegate_WhenTheTypedErrorResultIsAnError()
    {
        // Arrange
        var result = new Result<int, TestError>(new TestError("test error"));
        var successCalled = false;
        var errorCalled = false;
        const string successValue = "1";
        const string errorValue = "2";

        // Act
        var matchedResult = result.Match(
            _ =>
            {
                successCalled = true;
                return successValue;
            },
            _ =>
            {
                errorCalled = true;
                return errorValue;
            });

        // Assert
        Assert.False(successCalled);
        Assert.True(errorCalled);
        Assert.Equal(errorValue, matchedResult);
    }

    [Fact]
    public async Task MatchAsync_ExecutesSuccessDelegate_WhenTheTypedErrorResultIsSuccessful()
    {
        // Arrange
        var result = new Result<int, TestError>(42);
        var successCalled = false;
        var errorCalled = false;
        const string successValue = "1";
        const string errorValue = "2";

        // Act
        var matchedResult = await result.MatchAsync(
            _ =>
            {
                successCalled = true;
                return Task.FromResult<string>(successValue);
            },
            _ =>
            {
                errorCalled = true;
                return Task.FromResult<string>(errorValue);
            });

        // Assert
        Assert.True(successCalled);
        Assert.False(errorCalled);
        Assert.Equal(successValue, matchedResult);
    }

    [Fact]
    public async Task MatchAsync_ExecutesErrorDelegate_WhenTheTypedErrorResultIsAnError()
    {
        // Arrange
        var result = new Result<int, TestError>(new TestError("test error"));
        var successCalled = false;
        var errorCalled = false;
        const string successValue = "1";
        const string errorValue = "2";

        // Act
        var matchedResult = await result.MatchAsync(
            _ =>
            {
                successCalled = true;
                return Task.FromResult<string>(successValue);
            },
            _ =>
            {
                errorCalled = true;
                return Task.FromResult<string>(errorValue);
            });

        // Assert
        Assert.False(successCalled);
        Assert.True(errorCalled);
        Assert.Equal(errorValue, matchedResult);
    }

    [Fact]
    public void Switch_ExecutesSuccessDelegate_WhenTheTypedErrorResultIsSuccessful()
    {
        // Arrange
        Result<int, TestError> result = 42;
        var successCalled = false;
        var errorCalled = false;

        // Act
        result.Switch(
            _ => successCalled = true,
            _ => errorCalled = true);

        // Assert
        Assert.True(successCalled);
        Assert.False(errorCalled);
    }

    [Fact]
    public void Switch_ExecutesErrorDelegate_WhenTheTypedErrorResultIsAnError()
    {
        // Arrange
        var result = new Result<int, TestError>(new TestError("test error"));
        var successCalled = false;
        var errorCalled = false;

        // Act
        result.Switch(
            _ => successCalled = true,
            _ => errorCalled = true);

        // Assert
        Assert.False(successCalled);
        Assert.True(errorCalled);
    }

    [Fact]
    public async Task SwitchAsync_ExecutesSuccessDelegate_WhenTheTypedErrorResultIsSuccessful()
    {
        // Arrange
        var result = new Result<int, TestError>(42);
        var successCalled = false;
        var errorCalled = false;

        // Act
        await result.SwitchAsync(
            _ =>
            {
                successCalled = true;
                return Task.CompletedTask;
            },
            _ =>
            {
                errorCalled = true;
                return Task.CompletedTask;
            });

        // Assert
        Assert.True(successCalled);
        Assert.False(errorCalled);
    }

    [Fact]
    public async Task SwitchAsync_ExecutesErrorDelegate_WhenTheTypedErrorResultIsAnError()
    {
        // Arrange
        var result = new Result<int, TestError>(new TestError("test error"));
        var successCalled = false;
        var errorCalled = false;

        // Act
        await result.SwitchAsync(
            _ =>
            {
                successCalled = true;
                return Task.CompletedTask;
            },
            _ =>
            {
                errorCalled = true;
                return Task.CompletedTask;
            });

        // Assert
        Assert.False(successCalled);
        Assert.True(errorCalled);
    }
}