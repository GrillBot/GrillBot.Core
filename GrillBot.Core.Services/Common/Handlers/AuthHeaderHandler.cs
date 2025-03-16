using GrillBot.Core.Infrastructure.Auth;

namespace GrillBot.Core.Services.Common.Handlers;

public class AuthHeaderHandler(ICurrentUserProvider _currentUserProvider) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Options.TryGetValue(new("IsThirdParty"), out bool isThirdParty))
            isThirdParty = true;

        if (_currentUserProvider.IsLogged && !isThirdParty)
            request.Headers.Add("Authorization", _currentUserProvider.EncodedJwtToken);

        return base.SendAsync(request, cancellationToken);
    }
}
