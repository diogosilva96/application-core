using Microsoft.AspNetCore.Http;

namespace Application.Core.Api.Result.Mapping;

/// <summary>
/// Represents a mapper component that maps <see cref="ApiResult" /> to <see cref="IResult" />.
/// </summary>
internal class ApiResultMapper : IApiResultMapper
{
    /// <inheritdoc />
    public IResult Map(ApiResult apiResult)
    {
        ArgumentNullException.ThrowIfNull(apiResult);
        
        return apiResult.Match(success => success switch
            {
                Ok ok => Results.Ok(ok.Value),
                Created created => Results.Created(created.Uri, created.Value),
                NoContent => Results.NoContent(),
                Accepted accepted => Results.Accepted(accepted.Uri?.ToString(), accepted.Value),
                _ => throw new InvalidOperationException($"Unhandled success result type '{success.GetType()}'")
            },
            problemDetails => Results.Problem(problemDetails.Detail,
                problemDetails.Instance,
                problemDetails.Status,
                problemDetails.Title,
                problemDetails.Type,
                problemDetails.Extensions));
    }
}