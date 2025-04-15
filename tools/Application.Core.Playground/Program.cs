using Application.Core.Mediator;
using Application.Core.Playground;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

// decorator approach
serviceCollection//.AddTransient<IHandler<MyRequest, int>, MyHandler>()
                 .AddValidatorsFromAssembly(typeof(Program).Assembly)
                 .AddMediator(builder => builder
                                        .AddHandlersFromAssemblyContaining<Program>()
                                        .AddBehavior(typeof(ConsoleLogBehavior<,>))
                                        .AddBehavior(typeof(ValidationBehavior<,>))
                                        .AddBehavior(typeof(ModifyRequestBehavior<,>)));

var serviceProvider = serviceCollection.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();

for (var i = 0; i < 100000; i++)
{
    var request = new MyRequest()
    {
        Id = i,
        Message = "Hello"
    };
    using (new Measurer($"{nameof(IMediator)}.{nameof(IMediator.HandleAsync)}"))
    {
        var result = await mediator.HandleAsync(request, CancellationToken.None);
    }
}

MeasurementHolder.Instance.PrintResults();

Console.ReadLine();

public class MyHandler : IHandler<MyRequest, int>
{
    public Task<int> HandleAsync(MyRequest request, CancellationToken cancellationToken = default) => Task.FromResult<int>(request.Id);
}
public record MyRequest : IRequest<int>, IMessageRequest
{
    public required int Id { get; init; }
    
    public required string Message { get; set; }
}

public interface IMessageRequest
{  
    public string Message { get; set; }
}

public class ConsoleLogBehavior<TRequest, TResponse> : IHandlerBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Handling handler behavior {request}");
        return next();
    }
}

public class ValidationBehavior<TRequest, TResponse> : IHandlerBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Handling validaiton behavior {request}");
        return next();
    }
}

public class ModifyRequestBehavior<TRequest, TResponse> : IHandlerBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IMessageRequest
{
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        request.Message = "Look I modified the request message";
        Console.WriteLine($"Handling modify request behavior {request}");

        return next();
    }
}


