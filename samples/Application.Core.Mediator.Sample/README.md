# Application.Core.Mediator.Sample

This sample application demonstrates the usage of the `Application.Core.Mediator` library.

## Overview

The application includes the following components:

- **Request**: A `Ping` request that returns a `Pong` response.
- **Handler**: A `PingHandler` responsible for processing the `Ping` request.
- **Behaviors**: Two behaviors that decorate the handler:
    - `MeasurementBehavior`: Measures the time taken to handle the request.
    - `FluentValidationBehavior`: Automatically validates the request before it is processed.