using Microsoft.AspNetCore.Http;

namespace Application.Core.Api.Utils;

/// <summary>
/// Class for generating endpoint descriptors.
/// </summary>
internal static class EndpointDescriptors
{
    /// <summary>
    /// Creates an endpoint descriptor from the given <paramref name="request" />.
    /// </summary>
    /// <param name="request">The http request to generate the descriptor for.</param>
    /// <returns>The created endpoint descriptor.</returns>
    public static string For(HttpRequest request) => $"{request.Method} '{request.Path}'";
}