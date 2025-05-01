using Application.Core.Api.Result;

namespace Application.Core.Api.UnitTests.Result;

public class NoContentTests
{
    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenCallingEmptyConstructor()
    {
        // Arrange & Act
        var result = new NoContent();

        // Assert
        Assert.NotNull(result);
    }
}