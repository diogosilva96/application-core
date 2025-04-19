namespace Application.Core.Result.UnitTests;

public class ErrorTests
{
    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public void CreateError_ThrowsArgumentException_WhenEmptyOrWhitespaceMessageIsSpecified(string message)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Error(message!));
    }

    [Fact]
    public void CreateError_ThrowsArgumentException_WhenNullMessageIsSpecified()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Error(null!));
    }

    [Theory]
    [InlineData("my message")]
    [InlineData("abcd")]
    public void CreateError_CreatesErrorWithExpectedMessage(string message)
    {
        // Act
        var error = new Error(message);

        // Assert
        Assert.Equal(message, error.Message);
    }
}