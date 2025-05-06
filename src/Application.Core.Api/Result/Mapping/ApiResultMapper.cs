using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Core.Api.Result.Mapping;

/// <summary>
/// Represents a mapper component that maps <see cref="ApiResult" /> to <see cref="IResult" />.
/// </summary>
/// <param name="logger"></param>
/// <param name="httpContextAccessor"></param>
internal class ApiResultMapper(ILogger<ApiResultMapper> logger, IHttpContextAccessor httpContextAccessor) : IApiResultMapper
{
    /// <inheritdoc />
    public IResult Map(ApiResult apiResult)
    {
        return apiResult.Match(success =>
            {
                if (httpContextAccessor.HttpContext?.Request is not null)
                {
                    logger.LogInformation("Got {Success} for request {Request}", success, RequestDescriptors.For(httpContextAccessor.HttpContext.Request));
                }
                return success switch
                {
                    Ok ok => Results.Ok(ok.Value),
                    Created created => Results.Created(created.Uri, created.Value),
                    NoContent => Results.NoContent(),
                    Accepted accepted => Results.Accepted(accepted.Uri?.ToString(), accepted.Value),
                    _ => throw new InvalidOperationException($"Unhandled success result type '{success.GetType()}'")
                };
            },
            problemDetails =>
            {
                if (httpContextAccessor.HttpContext?.Request is not null)
                {
                    logger.LogWarning("Got {ProblemDetails} for request {Request}", problemDetails, RequestDescriptors.For(httpContextAccessor.HttpContext.Request));
                }
                return Results.Problem(problemDetails.Detail,
                    problemDetails.Instance,
                    problemDetails.Status,
                    problemDetails.Title,
                    problemDetails.Type,
                    problemDetails.Extensions);
            }
        );
    }
}