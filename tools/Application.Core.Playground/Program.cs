using System.Diagnostics;
using Application.Core.Mediator;
using Application.Core.Playground;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddTransient<IDomainHandler<MyRequest, int>, MyHandler>()
                 .AddTransient<IHandler<ConsoleLogRequest>, ConsoleLogRequestHandler>()
                 .AddTransient(typeof(IValidatableHandler<,>), typeof(ValidatableHandler<,>))
                 .AddTransient(typeof(IHandler<,>), typeof(Handler<,>))
                 .AddValidatorsFromAssembly(typeof(Program).Assembly)
                 .AddMediator();

var serviceProvider = serviceCollection.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();

// await mediator.HandleAsync(new ConsoleLogRequest()
// {
//     Message = "Wow, this is cool!"
// });

var request = new MyRequest()
{
    Id = 5
};
for (var i = 0; i < 10000; i++)
{
    using (new Measurer($"{nameof(IMediator)}.{nameof(IMediator.HandleAsync)}"))
    {
        var result = await mediator.HandleAsync(request, CancellationToken.None);
    }
}

MeasurementHolder.Instance.PrintResults();

// IHandler - entry point
// IValidatableHandler - 2nd handler
// IDomainHandler - 3rd handler (actual business logic)

Console.ReadLine();



public interface IDomainHandler<in TRequest> : IHandler<TRequest> where TRequest : IRequest
{
    
}

public interface IDomainHandler<in TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    
}

public class ConsoleLogRequest : IRequest
{
    public required string Message { get; init; }
}

public class ConsoleLogRequestHandler : IDomainHandler<ConsoleLogRequest>
{
    public Task HandleAsync(ConsoleLogRequest request, CancellationToken cancellationToken)
    {
     
        Console.WriteLine($"{DateTimeOffset.UtcNow}: {request.Message}");

        return Task.CompletedTask;
    }
}

public class MyRequest : IRequest<int>
{
    public required int Id { get; init; }
}
public class MyHandler : IDomainHandler<MyRequest, int>
{
    public Task<int> HandleAsync(MyRequest request, CancellationToken cancellationToken) => Task.FromResult(request.Id);
}

public interface IValidatableHandler<in TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
}

public class ValidatableHandler<TRequest, TResponse> : IValidatableHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
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
        //Console.WriteLine($"Handling {typeof(ValidatableHandler<,>)}");
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

public class Handler<TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IValidatableHandler<TRequest, TResponse> _nextHandler;

    public Handler(IValidatableHandler<TRequest, TResponse> nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //Console.WriteLine($"Handling {typeof(Handler<,>)}");
        return _nextHandler.HandleAsync(request, cancellationToken);
    }
}