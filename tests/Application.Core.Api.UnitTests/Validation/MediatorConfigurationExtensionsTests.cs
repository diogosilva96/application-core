using Application.Core.Api.Validation;
using Application.Core.Mediator;

namespace Application.Core.Api.UnitTests.Validation;

public class MediatorConfigurationExtensionsTests
{
    [Fact]
    public void AddValidationBehavior_AddsValidationBehaviorToServicesToRegister()
    {
        // Arrange
        var mediatorConfiguration = new MediatorConfiguration();

        // Act
        var result = mediatorConfiguration.AddValidationBehavior();

        // Assert
        Assert.Contains(result.ServicesToRegister, service => service.ServiceType == typeof(IBehavior<,>) &&
                                                              service.ImplementationType == typeof(ValidationBehavior<,>));
    }
}