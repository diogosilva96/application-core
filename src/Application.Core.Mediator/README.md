# Application.Core.Mediator

**Application.Core.Mediator** is a simple and lightweight mediator implementation for .NET Core applications. It provides a minimal set of features inspired by [MediatR](https://github.com/jbogard/MediatR), with a focus on clarity and ease of use.

## Features

- `IHandler<TRequest, TResponse>` – Defines a handler for a given request and response type.
- `IHandlerBehavior<TRequest, TResponse>` – Enables decorator-style behavior wrapping around handlers (e.g., logging, validation, etc.).
- `ISender` – The entry point for sending requests and triggering the appropriate handler(s).
- Automatic handler registration via assembly scanning (using reflection).

> 🧠 **Note:** The order in which behaviors are registered is important — they will be executed in the order they are added.

## Getting Started

For sample usage and implementation examples, check out the sample project:  
👉 [Application.Core.Mediator.Sample](https://github.com/diogosilva96/application-core/tree/main/samples/Application.Core.Mediator.Sample)

## Why?

After learning that the MediatR library would be moving to a paid model, I decided to build a lightweight version of it just for fun and giggles. 
I only needed a subset of MediatR's functionality in my own projects, so this was a great opportunity to:

- Implement just the features I actually use — keeping things simple and free of unnecessary complexity.
- Exercise my problem-solving skills by thinking through the core challenges and designing a clean and focused mediator style solution.

This project is not meant as a full replacement for MediatR, but as a fun and functional alternative for those who need something simpler.