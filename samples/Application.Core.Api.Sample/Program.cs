using Application.Core.Api.Result.Mapping;
using Application.Core.Api.Sample;
using Application.Core.Api.Validation;
using Application.Core.Mediator;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddLogging(b => b.AddConsole())
       .AddOpenApi()
       .AddSingleton(TimeProvider.System)
       .AddValidatorsFromAssemblyContaining<Program>();

// Application.Core.Api related services registration below
builder.Services
       .AddApiResultMapping()
       .AddMediator(c => c.AddValidationBehavior()
                          .AddHandlersFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("api/echo", async (ISender sender,IApiResultMapper resultMapper, [FromBody] EchoRequest request, CancellationToken cancellationToken) =>
   { 
       var apiResult = await sender.SendAsync(request, cancellationToken);
       return resultMapper.Map(apiResult);
   })
.WithName("Echo");

app.Run();
