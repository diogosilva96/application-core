# Application.Core.Mediator
Simple mediator implementation for .NET Core applications, with the following features:
- `IHandler` for handling requests
- `IHandlerBehavior` for decorating the request handling
- `ISender` for sending the requests to handle
- Automatic handler registration via assembly scanning (reflection)

Please note that the order in which the behaviors are registered is very important, as they will execute in the order they are registered.

For samples on how to use it, please see [Application.Core.Mediator.Sample](https://github.com/diogosilva96/application-core/tree/main/samples/Application.Core.Mediator.Sample).