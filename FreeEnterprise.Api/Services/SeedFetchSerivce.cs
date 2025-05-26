using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;


namespace FreeEnterprise.Api.Services;

public class SeedFetchSerivce(IHttpClientFactory httpClientFactory, ILogger<SeedFetchSerivce> logger) : ISeedFetchService
{
    public async Task<Response<string>> GetPatchHtmlAsync(SeedInformation seedInfo)
    {
        var validatedUrl = ValidateUrl(seedInfo.Url);
        if (!validatedUrl.Success)
        {
            return new Response<string>(string.Empty, errorMessage: validatedUrl.ErrorMessage, errorStatusCode: validatedUrl.ErrorStatusCode);
        }

        var client = httpClientFactory.CreateClient();

        try
        {
            var getResponse = await client.GetAsync(seedInfo.Url);
            if (!getResponse.IsSuccessStatusCode)
            {
                var errorMessage = await getResponse.Content.ReadAsStringAsync();
                logger.LogError("Failed to get patch html {errorMessage}", errorMessage);
                return new Response<string>().InternalServerError("Failed to get patch html");
            }

            var patchString = await getResponse.Content.ReadAsStringAsync();
            return Response.SetSuccess(patchString);
        }
        catch (Exception ex)
        {
            return new Response<string>().InternalServerError(ex.Message);
        }

    }

    private Response<Uri> ValidateUrl(string seedUrl)
    {
        if (!Uri.TryCreate(seedUrl, UriKind.Absolute, out var seedUri))
        {
            logger.LogWarning("Got {url} as a seed url, which is not an absolute URI", seedUrl);
            return new Response<Uri>().BadRequest("InvalidUri");
        }

        return seedUri switch
        {
#if DEBUG
            { Host: var h, Port: var p }
                when
                    (h.Equals("localhost") ||
                     h.Equals("127.0.0.1")) &&
                    p.Equals(8080) => new Response<Uri>().SetSuccess(seedUri),
#endif
            { Host: var h } when
                h.Equals("ff4fe.galeswift.com") ||
                h.Equals("ff4fe.com") => new Response<Uri>().SetSuccess(seedUri),

            _ => new Response<Uri>().BadRequest("Invalid URI host")
        };
    }

}
