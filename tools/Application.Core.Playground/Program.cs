using Application.Core.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddValidatorsFromAssembly(typeof(Program).Assembly)
                 .AddMediator(builder => builder.AddHandlersFromAssemblyContaining<Program>());

var serviceProvider = serviceCollection.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();

Console.ReadLine();