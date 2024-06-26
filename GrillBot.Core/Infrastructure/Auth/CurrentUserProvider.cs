﻿using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GrillBot.Core.Infrastructure.Auth;

public class CurrentUserProvider : ICurrentUserProvider
{
    private const string AUTH_HEADER_NAME = "Authorization";

    private readonly Dictionary<string, string> _headers;
    private JwtSecurityToken? _jwtToken;

    public string? EncodedJwtToken => _headers.TryGetValue(AUTH_HEADER_NAME, out var auth) ? auth : null;
    public bool IsLogged => _jwtToken is not null;
    public bool IsThirdParty => !string.IsNullOrEmpty(ThirdPartyKey);

    public string? Name => ReadClaim(ClaimTypes.Name) ?? ReadClaim("unique_name");
    public string? Id => ReadClaim(ClaimTypes.NameIdentifier) ?? ReadClaim("nameid");
    public string? Role => ReadClaim(ClaimTypes.Role) ?? ReadClaim("role");
    public string[] Permissions => ReadClaim("GrillBot:Permissions")?.Split(',') ?? Array.Empty<string>();
    public string? ThirdPartyKey => ReadClaim("GrillBot:ThirdPartyKey");

    [ExcludeFromCodeCoverage]
    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        var headers = httpContextAccessor.HttpContext?.Request?.Headers;
        _headers = (headers ?? new HeaderDictionary())
            .ToDictionary(o => o.Key, o => o.Value.ToString());

        _jwtToken = ReadJwtToken();
    }

    public CurrentUserProvider(Dictionary<string, string> headers)
    {
        _headers = headers;
        _jwtToken = ReadJwtToken();
    }

    private JwtSecurityToken? ReadJwtToken()
    {
        var token = EncodedJwtToken?.Replace("Bearer", "")?.Trim();
        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }

    private string? ReadClaim(string claimType)
        => !IsLogged ? null : (_jwtToken!.Claims.FirstOrDefault(o => o.Type == claimType)?.Value);

    public void SetCustomToken(string jwtToken)
    {
        if (_headers.ContainsKey(AUTH_HEADER_NAME))
            throw new InvalidOperationException("Unable assign JWT token if was provided from request.");

        _headers.Add(AUTH_HEADER_NAME, $"Bearer {jwtToken}");
        _jwtToken = ReadJwtToken();
    }
}
