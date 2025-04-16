# # Application.Core.Mediator.Sample
Sample application demonstrating the usage of the `Application.Core.Mediator` library.

The application has a request of type `Ping` that returns `Pong` and a handler of type `PingHandler`.
Two behaviors are used to decorate the handler:
- A `MeasurementBehavior` for measuring how long it takes to handle the request
- A `FluentValidationBehavior` for automatically validating the request before handling it