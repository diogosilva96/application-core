namespace Application.Core.Result.UnitTests;

public class ResultTests
{
    [Fact]
    public void CreateResult_CreatesSuccessfulTypedErrorResult_WhenAValueIsSpecifiedUsingImplicitConversion()
    {
        // Arrange
        const string value = "test";

        // Act
        Result<string, Error> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void CreateResult_CreatesErrorTypedErrorResult_WhenAnErrorIsSpecifiedUsingImplicitConversion()
    {
        // Arrange
        var error = new Error("test error");

        // Act
        Result<string, Error> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void CreateResult_CreatesSuccessfulTypedErrorResult_WhenAValueIsSpecified()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = new Result<string, Error>(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void CreateResult_ThrowsArgumentNullForTypedErrorResult_WhenNoValueIsSpecified()
    {
        // Arrange
        string value = null!;

        // Act & Assert 
        Assert.Throws<ArgumentNullException>(() => new Result<string, Error>(value));
    }

    [Fact]
    public void CreateResult_CreatesErrorTypedErrorResult_WhenAnErrorIsSpecified()
    {
        // Arrange
        var error = new Error("test error");

        // Act
        var result = new Result<string, Error>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void CreateResult_ThrowsArgumentNullForErrorTypedResult_WhenNoErrorIsSpecified()
    {
        // Arrange
        Error error = null!;

        // Act & Assert 
        Assert.Throws<ArgumentNullException>(() => new Result<string, Error>(error));
    }

    [Fact]
    public void Match_ExecutesSuccessDelegate_WhenTheTypedErrorResultIsSuccessful()
    {
        // Arrange
        var result = new Result<int, Error>(42);
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
        var result = new Result<int, Error>(new Error("test error"));
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
        var result = new Result<int, Error>(42);
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
        var result = new Result<int, Error>(new Error("test error"));
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
        Result<int, Error> result = 42;
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
        var result = new Result<int, Error>(new Error("test error"));
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
        var result = new Result<int, Error>(42);
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
        var result = new Result<int, Error>(new Error("test error"));
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

    [Fact]
    public void CreateResult_CreatesSuccessfulResult_WhenAValueIsSpecifiedUsingImplicitConversion()
    {
        // Arrange
        const string value = "test";

        // Act
        Result<string> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void CreateResult_CreatesErrorResult_WhenAnErrorIsSpecifiedUsingImplicitConversion()
    {
        // Arrange
        var error = new Error("test error");

        // Act
        Result<string> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void CreateResult_CreatesSuccessfulResult_WhenAValueIsSpecified()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = new Result<string>(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void CreateResult_ThrowsArgumentNull_WhenNoValueIsSpecified()
    {
        // Arrange
        string value = null!;

        // Act & Assert 
        Assert.Throws<ArgumentNullException>(() => new Result<string>(value));
    }

    [Fact]
    public void CreateResult_CreatesErrorResult_WhenAnErrorIsSpecified()
    {
        // Arrange
        var error = new Error("test error");

        // Act
        var result = new Result<string>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void CreateResult_ThrowsArgumentNull_WhenNoErrorIsSpecified()
    {
        // Arrange
        Error error = null!;

        // Act & Assert 
        Assert.Throws<ArgumentNullException>(() => new Result<string>(error));
    }

    [Fact]
    public void Match_ExecutesSuccessDelegate_WhenTheResultIsSuccessful()
    {
        // Arrange
        Result<int> result = 42;
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
    public void Match_ExecutesErrorDelegate_WhenTheResultIsAnError()
    {
        // Arrange
        Result<int> result = new Error("test error");
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
    public async Task MatchAsync_ExecutesSuccessDelegate_WhenTheResultIsSuccessful()
    {
        // Arrange
        Result<int> result = 42;
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
    public async Task MatchAsync_ExecutesErrorDelegate_WhenTheResultIsAnError()
    {
        // Arrange
        Result<int> result = new Error("test error");
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
    public void Switch_ExecutesSuccessDelegate_WhenTheResultIsSuccessful()
    {
        // Arrange
        Result<int> result = 42;
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
    public void Switch_ExecutesErrorDelegate_WhenTheResultIsAnError()
    {
        // Arrange
        Result<int> result = new Error("test error");
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
    public async Task SwitchAsync_ExecutesSuccessDelegate_WhenTheResultIsSuccessful()
    {
        // Arrange
        Result<int> result = 42;
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
    public async Task SwitchAsync_ExecutesErrorDelegate_WhenTheResultIsAnError()
    {
        // Arrange
        Result<int> result = new Error("test error");
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