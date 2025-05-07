using Application.Core.Api.Result;
using Application.Core.Api.Result.Mapping;
using Application.Core.Mediator;
using Microsoft.AspNetCore.Http;

namespace Application.Core.Api.RequestProcessing;

/// <summary>
/// Represents a component responsible for processing API requests.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="resultMapper">The result mapper.</param>
internal class ApiRequestProcessor(ISender sender, IApiResultMapper resultMapper) : IApiRequestProcessor
{
    /// <inheritdoc />
    public async Task<IResult> ProcessAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<ApiResult>
    {
        var apiResult = await sender.SendAsync(request, cancellationToken);
        return resultMapper.Map(apiResult);
    }
}