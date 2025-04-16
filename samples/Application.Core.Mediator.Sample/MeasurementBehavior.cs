using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Core.Mediator.Sample;

public class MeasurementBehavior<TRequest, TResponse>(ILogger<MeasurementBehavior<TRequest, TResponse>> logger) : IHandlerBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Measuring request {Request}", request);
        var startTime = Stopwatch.GetTimestamp();
        var response = await next();
        var endTime = Stopwatch.GetTimestamp();
        var elapsedTimeInMilliseconds = Stopwatch.GetElapsedTime(startTime, endTime);
        logger.LogInformation("Request {Request} took {ElapsedMilliseconds} ms", request.GetType().Name, elapsedTimeInMilliseconds);
        return response;
    }
}