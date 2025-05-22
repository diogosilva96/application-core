using Application.Core.Api.Validation;

namespace Application.Core.Api.Sample;

public class EchoValidationFailurePropertyMapper : ValidationFailurePropertyMapperBase
{
    public EchoValidationFailurePropertyMapper()
    {
        ConfigurePropertyMappingsForType<EchoRequest>();
    }
}