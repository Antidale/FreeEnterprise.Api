using Microsoft.AspNetCore.Authentication;

namespace FreeEnterprise.Api.Auth;

public class ApiKeyAuthOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
    //TODO: after initial implementation, update to more standard X-API-KEY
    //requires updating bots as well
    public const string HeaderName = "Api-Key";
}
