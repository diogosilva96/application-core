using Application.Core.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddTransient<IDomainHandler<int, int>, MyHandler>()
                 .AddTransient(typeof(IValidatableHandler<,>), typeof(ValidatableHandler<,>))
                 .AddTransient(typeof(IHandler<,>), typeof(Handler<,>))
                 .AddValidatorsFromAssembly(typeof(Program).Assembly);

var serviceProvider = serviceCollection.BuildServiceProvider();


// IHandler - entry point
// IValidatableHandler - 2nd handler
// IDomainHandler - 3rd handler (actual business logic)

var result = await serviceProvider.GetRequiredService<IHandler<int, int>>()
                                  .HandleAsync(1, CancellationToken.None);

Console.ReadLine();


public interface IDomainHandler<in TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    
}

public class MyHandler : IDomainHandler<int, int>
{
    public Task<int> HandleAsync(int request, CancellationToken cancellationToken) => Task.FromResult(request);
}

public interface IValidatableHandler<in TRequest, TResponse> : IHandler<TRequest, TResponse>
{
}

public class ValidatableHandler<TRequest, TResponse> : IValidatableHandler<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IDomainHandler<TRequest, TResponse> _nextHandler;

    public ValidatableHandler(IEnumerable<IValidator<TRequest>> validators, IDomainHandler<TRequest, TResponse> nextHandler)
    {
        _validators = validators;
        _nextHandler = nextHandler;
    }

    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling {typeof(ValidatableHandler<,>)}");
        var validationTasks = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationTasks.Where(r => !r.IsValid)
                                    .SelectMany(r => r.Errors)
                                    .ToArray();

        if (errors.Length > 0)
        {
            throw new ValidationException(errors); 
        }

        return await _nextHandler.HandleAsync(request, cancellationToken);
    }
}

public class Handler<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    private readonly IValidatableHandler<TRequest, TResponse> _nextHandler;

    public Handler(IValidatableHandler<TRequest, TResponse> nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Console.WriteLine($"Handling {typeof(Handler<,>)}");
        return _nextHandler.HandleAsync(request, cancellationToken);
    }
}