using Application.Core.Api.Result;
using Application.Core.Api.UnitTests.Validation.Utils;
using Application.Core.Api.Validation;
using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace Application.Core.Api.UnitTests.Validation;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task HandleAsync_ExecutesNextDelegate_WhenAValidRequestIsGivenAndThereAreRequestValidators()
    {
        // Arrange
        var expectedResult = new Ok();
        var validators = new List<IValidator<TestRequest>>
        {
            Substitute.For<IValidator<TestRequest>>(),
            Substitute.For<IValidator<TestRequest>>()
        };
        foreach (var validator in validators)
        {
            validator.ValidateAsync(Arg.Any<TestRequest>(), Arg.Any<CancellationToken>())
                     .ReturnsForAnyArgs(new ValidationResult());
        }

        var next = Substitute.For<Func<Task<ApiResult>>>();
        next.Invoke().ReturnsForAnyArgs(expectedResult);
        var behavior = new ValidationBehavior<TestRequest, ApiResult>(validators);
        var request = new TestRequest();

        // Act
        var result = await behavior.HandleAsync(request, next, TestContext.Current.CancellationToken);

        // Assert
        await next.ReceivedWithAnyArgs(1).Invoke();
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult, result.Value);
    }

    [Fact]
    public async Task HandleAsync_ExecutesNextDelegate_WhenAValidRequestIsGivenAndThereAreNoRequestValidators()
    {
        // Arrange
        var expectedResult = new Ok();
        var next = Substitute.For<Func<Task<ApiResult>>>();
        next.Invoke().ReturnsForAnyArgs(expectedResult);
        var behavior = new ValidationBehavior<TestRequest, ApiResult>([]);
        var request = new TestRequest();

        // Act
        var result = await behavior.HandleAsync(request, next, TestContext.Current.CancellationToken);

        // Assert
        await next.ReceivedWithAnyArgs(1).Invoke();
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult, result.Value);
    }

    [Fact]
    public async Task HandleAsync_ReturnsBadRequest_WhenAnInvalidRequestIsGiven()
    {
        // Arrange
        var fixture = new Fixture();
        var expectedValidationFailures = new List<ValidationFailure>();
        var validators = new List<IValidator<TestRequest>>
        {
            Substitute.For<IValidator<TestRequest>>(),
            Substitute.For<IValidator<TestRequest>>()
        };
        foreach (var validator in validators)
        {
            var validationFailures = fixture.CreateMany<ValidationFailure>().ToArray();
            validator.ValidateAsync(Arg.Any<TestRequest>(), Arg.Any<CancellationToken>())
                     .ReturnsForAnyArgs(new ValidationResult(validationFailures));
            expectedValidationFailures.AddRange(validationFailures);
        }

        var next = Substitute.For<Func<Task<ApiResult>>>();
        var behavior = new ValidationBehavior<TestRequest, ApiResult>(validators);
        var request = new TestRequest();

        // Act
        var result = await behavior.HandleAsync(request, next, TestContext.Current.CancellationToken);

        // Assert
        await next.DidNotReceiveWithAnyArgs().Invoke();
        Assert.True(result.IsError);
        Assert.IsType<BadRequest>(result.Error);
        var badRequest = (BadRequest)result.Error;
        Assert.All(expectedValidationFailures, expectedFailure =>
            Assert.Contains(badRequest.Errors, error => error.Key == expectedFailure.PropertyName &&
                                                        error.Value.Contains(expectedFailure.ErrorMessage)));
    }
}