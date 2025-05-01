using Application.Core.Api.Result;

namespace Application.Core.Api.UnitTests.Result;

public class OkTests
{
    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenCallingEmptyConstructor()
    {
        // Arrange & Act
        var result = new Ok();

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenValueIsSpecified()
    {
        // Arrange
        var value = new object();

        // Act
        var result = new Ok(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }
}