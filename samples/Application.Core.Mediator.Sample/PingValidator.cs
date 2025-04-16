using FluentValidation;

namespace Application.Core.Mediator.Sample;

public class PingValidator : AbstractValidator<Ping>
{
    public PingValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.TimestampUtc).GreaterThan(timeProvider.GetUtcNow().AddMinutes(-1));
    }
}