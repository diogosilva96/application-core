using Application.Core.Api.Validation;

namespace Application.Core.Api.UnitTests.Utils;

public class TestValidationFailurePropertyMapper : ValidationFailurePropertyMapperBase
{
    public new void ConfigureConditionalPropertyMapping(Func<string, bool> propertyNamePredicate, string mappedPropertyName)
    {
        base.ConfigureConditionalPropertyMapping(propertyNamePredicate, mappedPropertyName);
    }

    public new void ConfigurePropertyMapping(string propertyName, string mappedPropertyName)
    {
        base.ConfigurePropertyMapping(propertyName, mappedPropertyName);
    }

    public new void ConfigurePropertyMappingsForType<T>()
    {
        base.ConfigurePropertyMappingsForType<T>();
    }
}