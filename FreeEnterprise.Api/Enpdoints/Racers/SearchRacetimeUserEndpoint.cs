using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class SearchRacetimeUserEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapSearchRacetimeUser()
        {
            app.MapGet(ApiEndpoints.Racers.SearchRacetimeUser, SearchRacetimeUserAsync)
                .WithTags(OpenApiMetadata.Racers.ToString())
                .WithOrder((int)OpenApiMetadata.Racers)
                .ProducesValidationProblem();
            return app;
        }
    }

    public static async Task<IResult> SearchRacetimeUserAsync
    (
        [FromServices] IMemoryCache memoryCache,
        [FromServices] IRacetimeDataService dataService,
        HttpContext httpContext
    )
    {
        List<string> validKeys = ["name", "discriminator", "term"];
        var validPairs = httpContext.Request.Query.Where(x => validKeys.Contains(x.Key)).Select(x => $"{x.Key}={x.Value}");
        if (!validPairs.Any())
        {
            return Results.BadRequest("Must include one of name, discriminator, or term as a query parameter");
        }

        var querystring = $"?{string.Join('&', validPairs)}";
        if (memoryCache.TryGetValue<string>(querystring, out var cacheResponse) && cacheResponse is not null)
        {
            return TypedResults.Ok(cacheResponse);
        }

        var response = await dataService.SearchRacetimeUserAsync(querystring);
        if (response.Success)
        {
            memoryCache.SetCache(querystring, response.Data);
        }

        return response.ToResult();
    }
}
