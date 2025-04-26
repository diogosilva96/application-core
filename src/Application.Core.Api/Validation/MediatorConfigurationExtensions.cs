using Application.Core.Mediator;

namespace Application.Core.Api.Validation;

/// <summary>
/// Class that contains extension methods for <see cref="MediatorConfiguration" />.
/// </summary>
public static class MediatorConfigurationExtensions
{
    /// <summary>
    /// Adds the validation behavior to the given <paramref name="mediatorConfiguration" />.
    /// </summary>
    /// <param name="mediatorConfiguration">The mediator configuration to add the behavior for.</param>
    /// <returns>The <see cref="MediatorConfiguration" /> with the configured validation behavior.</returns>
    public static MediatorConfiguration AddValidationBehavior(this MediatorConfiguration mediatorConfiguration) =>
        mediatorConfiguration.AddBehavior(typeof(ValidationBehavior<,>));
}