using Application.Core.Mediator;
using Application.Core.Mediator.Sample;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole())
        .AddValidatorsFromAssemblyContaining<Program>()
        .AddSingleton(TimeProvider.System);

// mediator registration
services.AddMediator(configurator => configurator
                                     // Behavior registration
                                     // Important: behavior registration order matters, so make sure to add them in the order you want them to execute
                                     .AddBehavior(typeof(MeasurementBehavior<,>))
                                     .AddBehavior(typeof(FluentValidationBehavior<,>))
                                     // Handler registration
                                     .AddHandlersFromAssemblyContaining<Program>());
                                     
await using var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
// resolve the mediator
var mediator = serviceProvider.GetRequiredService<IMediator>();

// actually handle the request
var ping = new Ping { TimestampUtc = DateTimeOffset.UtcNow };
var pong = await mediator.HandleAsync(ping);

logger.LogInformation("Got response {Pong} from mediator for request {Ping}", pong, ping);

Console.ReadKey();
