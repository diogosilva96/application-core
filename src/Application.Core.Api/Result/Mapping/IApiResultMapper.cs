using Microsoft.AspNetCore.Http;

namespace Application.Core.Api.Result.Mapping;

/// <summary>
/// Represents a mapper abstraction that maps <see cref="ApiResult" /> to <see cref="IResult" />.
/// </summary>
public interface IApiResultMapper
{
    /// <summary>
    /// Maps the specified <see cref="ApiResult" /> to an <see cref="IResult" />.
    /// </summary>
    /// <param name="apiResult">The api result to map.</param>
    /// <returns>The mapped <see cref="IResult" />.</returns>
    public IResult Map(ApiResult apiResult);
}