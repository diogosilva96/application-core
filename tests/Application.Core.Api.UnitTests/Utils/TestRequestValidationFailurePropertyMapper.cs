using Application.Core.Api.Validation;

namespace Application.Core.Api.UnitTests.Utils;

public class TestRequestValidationFailurePropertyMapper : ValidationFailurePropertyMapperBase
{
    public void ConfigureMapping(string propertyName, string mappedPropertyName)
    {
        ConfigurePropertyMapping(propertyName, mappedPropertyName);
    }
}