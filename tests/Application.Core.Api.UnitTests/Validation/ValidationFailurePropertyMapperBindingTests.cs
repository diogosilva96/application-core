using Application.Core.Api.UnitTests.Utils;
using Application.Core.Api.Validation;

namespace Application.Core.Api.UnitTests.Validation;

public class ValidationFailurePropertyMapperBindingTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance_WithValidMapperType()
    {
        // Arrange
        var mapperType = typeof(TestRequestValidationFailurePropertyMapper);

        // Act
        var binding = new ValidationFailurePropertyMapperBinding(mapperType);

        // Assert 
        Assert.Equal(mapperType, binding.MapperType);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WithNonMapperType()
    {
        // Arrange
        var invalidType = typeof(TestRequest);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ValidationFailurePropertyMapperBinding(invalidType));
    }
}