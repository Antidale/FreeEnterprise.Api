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

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.ContentType = "application/problem+json";
        var problemDetailsService = Context.RequestServices.GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = Context,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "A valid API Key is required. This application uses a non-standard header: Api-Key",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2"
            }
        });
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        Response.ContentType = "application/problem+json";
        var problemDetailsService = Context.RequestServices.GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = Context,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Unauthorized",
                Detail = "Invalid API key provided.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2"
            }
        });
    }
}
