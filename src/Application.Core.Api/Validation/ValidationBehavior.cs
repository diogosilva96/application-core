using Application.Core.Api.Result;
using Application.Core.Mediator;
using FluentValidation;

namespace Application.Core.Api.Validation;

/// <summary>
/// Represents a behavior that validates requests.
/// </summary>
/// <param name="validators">The request validators.</param>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ApiResult
{
    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        var validationTasks = validators.Select(x => x.ValidateAsync(request, cancellationToken))
                                        .ToArray();

        await Task.WhenAll(validationTasks);
        var validationErrors = validationTasks.Where(t => !t.Result.IsValid)
                                              .SelectMany(t => t.Result.Errors)
                                              .ToArray();
        if (validationErrors.Length > 0)
        {
            return (TResponse)new BadRequest($"Validation failed for request of type '{typeof(TRequest).Name}'.", validationErrors);
        }

        return await next();
    }
}