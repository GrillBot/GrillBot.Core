using System.Net.Http.Json;

namespace GrillBot.Core.Services.Common.Extensions;

public static class HttpMethodExtensions
{
    public static HttpRequestMessage ToRequest(this HttpMethod method, string uri)
        => new(method, uri);

    public static HttpRequestMessage ToRequest<T>(this HttpMethod method, string uri, T body)
        => new(method, uri) { Content = JsonContent.Create(body) };
}
