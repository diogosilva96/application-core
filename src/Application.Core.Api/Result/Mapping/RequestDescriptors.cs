using Microsoft.AspNetCore.Http;

namespace Application.Core.Api.Result.Mapping;

/// <summary>
/// Class for generating request descriptors.
/// </summary>
internal static class RequestDescriptors
{
    /// <summary>
    /// Creates a request descriptor from the given <paramref name="request" />.
    /// </summary>
    /// <param name="request">The http request to generate the descriptor for.</param>
    /// <returns>The created request descriptor.</returns>
    public static string For(HttpRequest request) => $"{request.Method} '{request.Path}'";
}