using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.Core.Mediator.Sample;

public class FluentValidationBehavior<TRequest, TResponse>(ILogger<FluentValidationBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators) : IBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Validating request {Request}", request);
        
        var validationResults = validators.Select(v => v.ValidateAsync(request, cancellationToken)).ToArray();
        await Task.WhenAll(validationResults);
        var validationErrors = validationResults.Where(x => !x.Result.IsValid)
                                                .SelectMany(x => x.Result.Errors)
                                                .ToArray();
        if (validationErrors.Length > 0)
        {
            // the exception being thrown is just an example
            // another alternative could be to return it to some type that reflects a validation error (preferably by also using a generic filter on TResponse)
            throw new ValidationException($"Validation failed for {typeof(TRequest).Name}.", validationErrors);
        }
        
        return await next();
    }
}