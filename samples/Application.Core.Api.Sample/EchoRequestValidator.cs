using FluentValidation;

namespace Application.Core.Api.Sample;

public class EchoRequestValidator : AbstractValidator<EchoRequest>
{
    public EchoRequestValidator()
    {
        RuleFor(x => x.Message).NotEmpty()
                               .MinimumLength(5)
                               .MaximumLength(255);
    }
}