using Application.Core.Api.Result;

namespace Application.Core.Api.Sample;

public class EchoRequestHandler(TimeProvider timeProvider, ILogger<EchoRequestHandler> logger) : IApiHandler<EchoRequest>
{
    public Task<ApiResult> HandleAsync(EchoRequest request, CancellationToken cancellationToken = default)
    {
        var number = Random.Shared.Next(0, 10);
        if (number >= 7)
        {
            logger.LogWarning("Number {Number} is greater than or equal to 7 => simulating an error.", number);
            return Task.FromResult<ApiResult>(new InternalServerError("Random error occurred."));
        }
        
        var response = new EchoResponse($"Received: {request.Message}", timeProvider.GetUtcNow());
        return Task.FromResult<ApiResult>(new Ok(response));
    }
}