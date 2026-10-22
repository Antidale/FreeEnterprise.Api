using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FreeEnterprise.Api.Auth;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration
) : AuthenticationHandler<ApiKeyAuthOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyAuthOptions.HeaderName, out var headersKey))
        {
            return AuthenticateResult.NoResult();
        }

        if (string.IsNullOrWhiteSpace(headersKey))
        {
            return AuthenticateResult.Fail($"{ApiKeyAuthOptions.HeaderName} present but assigned no value");
        }

        var configuredKey = configuration.GetValue<string>(ApiKeyAuthOptions.HeaderName);

        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            return AuthenticateResult.Fail("API Key not configured");
        }

        var headersKeyBytes = Encoding.UTF8.GetBytes(headersKey!).AsSpan();
        var configuredKeyBytes = Encoding.UTF8.GetBytes(configuredKey).AsSpan();

        if (!CryptographicOperations.FixedTimeEquals(configuredKeyBytes, headersKeyBytes))
        {
            return AuthenticateResult.Fail("Invalid API Key");
        }

        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, ApiKeyAuthOptions.DefaultScheme)], ApiKeyAuthOptions.DefaultScheme);

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), ApiKeyAuthOptions.DefaultScheme));

    }
}
