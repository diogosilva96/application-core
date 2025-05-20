using Application.Core.Api.Result;
using Application.Core.Api.Result.Mapping;
using Application.Core.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Core.Api.RequestProcessing;

/// <summary>
/// Represents a component responsible for processing API requests.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="resultMapper">The result mapper.</param>
internal class ApiRequestProcessor(ISender sender, IApiResultMapper resultMapper, ILogger<ApiRequestProcessor> logger) : IApiRequestProcessor
{
    /// <inheritdoc />
    public async Task<IResult> ProcessAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<ApiResult>
    {
        var apiResult = await sender.SendAsync(request, cancellationToken);
        
        apiResult.Switch(success => logger.LogInformation("{RequestType} resulted in {SuccessResultType}", request.GetType().Name, success.GetType().Name),
            problemDetails => logger.LogWarning("{RequestType} resulted in {FailureResultType}. Details: {Details}", request.GetType().Name, problemDetails.GetType().Name, problemDetails.ToDetailedErrorMessage()));
        
        return resultMapper.Map(apiResult);
    }
}