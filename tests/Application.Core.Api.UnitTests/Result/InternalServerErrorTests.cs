using Application.Core.Api.Result;
using AutoFixture;

namespace Application.Core.Api.UnitTests.Result;

public class InternalServerErrorTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_CreatesCorrectInstance()
    {
        // Arrange
        var message = _fixture.Create<string>();

        // Act
        var result = new InternalServerError(message);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.Status);
        Assert.Empty(result.Extensions);
    }
}