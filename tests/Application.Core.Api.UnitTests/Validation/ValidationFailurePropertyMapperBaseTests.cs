using Application.Core.Api.UnitTests.Utils;
using Application.Core.Api.Validation;

namespace Application.Core.Api.UnitTests.Validation;

public class ValidationFailurePropertyMapperBaseTests
{
    [Fact]
    public void ConfigurePropertyMapper_ShouldReturnMappedPropertyName()
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();
        const string propertyName = "TestProperty";
        const string expectedPropertyName = "MappedTestProperty";
        
        // Act
        mapper.ConfigurePropertyMapping(propertyName, expectedPropertyName);

        // Assert
        Assert.Equal(expectedPropertyName, mapper[propertyName]);
    }

    [Fact]
    public void ConfigureConditionalPropertyMapping_ShouldReturnMappedPropertyName()
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();
        const string propertyName = "TestSomething";
        const string expectedPropertyName = "MappedConditionalProperty";
        
        // Act
        mapper.ConfigureConditionalPropertyMapping(name => name.StartsWith("test", StringComparison.OrdinalIgnoreCase), expectedPropertyName);

        // Assert
        Assert.Equal(expectedPropertyName, mapper[propertyName]);
    }
    
    [Fact]
    public void ConfigurePropertyMappingsForType_ShouldMapAllPropertiesToTheirNames()
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();
        var expectedPropertyNames = new[] { nameof(TestRequest.Id) };

        // Act
        mapper.ConfigurePropertyMappingsForType<TestRequest>();

        // Assert
        Assert.All(expectedPropertyNames, expectedPropertyName => Assert.Equal(expectedPropertyName, mapper[expectedPropertyName]));
    }

    [Fact]
    public void Indexer_ShouldReturnDefaultPropertyName_WhenThePropertyHasNoMappingDefined()
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();
        const string propertyName = "UnmappedProperty";
        const string expectedPropertyName = PropertyNames.Unknown;

        // Act
        var result = mapper[propertyName];

        // Assert
        Assert.Equal(expectedPropertyName, result);
    }

    [Theory]
    [ClassData(typeof(EmptyOrWhiteSpaceOrNullStringTestData))]
    public void ConfigurePropertyMapping_ShouldThrowException_WhenPropertyNameIsInvalid(string? propertyName)
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();

        // Act
        var exception = Record.Exception(() => mapper.ConfigurePropertyMapping(propertyName!, "MappedValue"));
        
        // Assert
        Assert.NotNull(exception);
        Assert.True(exception is ArgumentException or ArgumentNullException);
    }

    [Theory]
    [ClassData(typeof(EmptyOrWhiteSpaceOrNullStringTestData))]
    public void ConfigurePropertyMapping_ShouldThrowException_WhenMappedPropertyNameIsInvalid(string? mappedPropertyName)
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();

        // Act
        var exception = Record.Exception(() => mapper.ConfigurePropertyMapping("prop", mappedPropertyName!));
        
        // Assert
        Assert.NotNull(exception);
        Assert.True(exception is ArgumentException or ArgumentNullException);
    }

    [Fact]
    public void ConfigureConditionalPropertyMapping_ShouldThrowArgumentNullException_WhenPredicateIsNull()
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mapper.ConfigureConditionalPropertyMapping(null!, "prop"));
    }

    [Theory]
    [ClassData(typeof(EmptyOrWhiteSpaceOrNullStringTestData))]
    public void ConfigureConditionalPropertyMapping_ShouldThrowException_WhenMappedPropertyNameIsInvalid(string? mappedPropertyName)
    {
        // Arrange
        var mapper = new TestValidationFailurePropertyMapper();

        // Act
        var exception = Record.Exception(() => mapper.ConfigureConditionalPropertyMapping(x => x.Equals("a"), mappedPropertyName!));
        
        // Assert
        Assert.NotNull(exception);
        Assert.True(exception is ArgumentException or ArgumentNullException);
    }

    private class EmptyOrWhiteSpaceOrNullStringTestData : TheoryData<string?>
    {
        public EmptyOrWhiteSpaceOrNullStringTestData()
        {
            AddRange(string.Empty, "   ", null);
        }
    }
}