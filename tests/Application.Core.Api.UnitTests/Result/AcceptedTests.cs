using Application.Core.Api.Result;
using AutoFixture;

namespace Application.Core.Api.UnitTests.Result;

public class AcceptedTests
{
    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenCallingEmptyConstructor()
    {
        // Arrange & Act
        var result = new Accepted();

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Value);
        Assert.Null(result.Uri);
    }

    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenValueIsSpecified()
    {
        // Arrange
        var value = new object();

        // Act
        var result = new Accepted(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Uri);
    }

    [Fact]
    public void Constructor_CreatesCorrectInstance_WhenValueAndUriIsSpecified()
    {
        // Arrange
        var fixture = new Fixture();
        var value = new object();
        var uri = fixture.Create<Uri>();

        // Act
        var result = new Accepted(value, uri);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
        Assert.Equal(uri, result.Uri);
    }
}