using GrillBot.Core.Infrastructure.Auth;
using GrillBot.Core.Managers.Performance;

namespace GrillBot.Core.Services.Common.Handlers;

public class HttpClientHandler(
    ICurrentUserProvider _currentUserProvider,
    ICounterManager _counterManager
) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var serviceName = GetServiceName(request);

        using (_counterManager.Create($"Service.{serviceName}"))
        {
            SetAuthorizationToken(request);
            return await base.SendAsync(request, cancellationToken);
        }
    }

    private void SetAuthorizationToken(HttpRequestMessage request)
    {
        if (!request.Options.TryGetValue(new("IsThirdParty"), out bool isThirdParty))
            isThirdParty = true;

        if (_currentUserProvider.IsLogged && !isThirdParty)
            request.Headers.Add("Authorization", _currentUserProvider.EncodedJwtToken);
    }

    private static string GetServiceName(HttpRequestMessage request)
    {
        return request.Options.TryGetValue(new("ServiceName"), out string? serviceName) && !string.IsNullOrEmpty(serviceName)
            ? serviceName
            : $"({request.RequestUri})";
    }
}
