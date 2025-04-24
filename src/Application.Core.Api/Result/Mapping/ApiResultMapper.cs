using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Core.Api.Result.Mapping;

/// <summary>
/// Represents a mapper component that maps <see cref="ApiResult" /> to <see cref="IResult" />.
/// </summary>
/// <param name="logger"></param>
/// <param name="httpContext"></param>
internal class ApiResultMapper(ILogger<ApiResultMapper> logger, HttpContext httpContext) : IApiResultMapper
{
    /// <inheritdoc />
    public IResult Map(ApiResult apiResult)
    {
        return apiResult.Match(success =>
            {
                logger.LogInformation("Got {Success} for request {Request}", success, RequestDescriptors.For(httpContext.Request));
                return success switch
                {
                    Ok ok => Results.Ok(ok.Value),
                    Created created => Results.Created(created.Uri, created.Value),
                    NoContent _ => Results.NoContent(),
                    Accepted accepted => Results.Accepted(accepted.Uri?.ToString(), accepted.Value),
                    _ => throw new InvalidOperationException($"Unhandled success result type '{success.GetType()}'")
                };
            },
            problemDetails =>
            {
                logger.LogWarning("Got {ProblemDetails} for request {Request}", problemDetails, RequestDescriptors.For(httpContext.Request));
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