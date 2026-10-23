using System;
using System.ComponentModel;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class GetRacersEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetRacersEndpoint()
        {
            app.MapGet(ApiEndpoints.Racers.GetAll, GetRacersAsync)
                .WithTags(OpenApiMetadata.Racers.ToString())
                .WithOrder((int)OpenApiMetadata.Racers)
                .WithDescription("Results are cached for 5 minutes, based on the combination of the offset, limit, and name query parameters");
            return app;
        }
    }

    public static async Task<IResult> GetRacersAsync
    (
        [FromServices] IMemoryCache memoryCache,
        [FromServices] IRacerRepository repository,
        [Description("used with limit to accomplish paging through results")]
        [FromQuery] int offset = 0,
        [Description("used with offset to accomplish paging through results. Currently no maximum value, but there might be a limit imposed at some point in the future")]
        [FromQuery] int limit = 20,
        [Description("The name of the racer, currently looks at either their racetime name or the name of the twitch account linked to their discord account. search is case insensitive, and all names starting with your search string are returned, within the offset and limit parameters.")]
        [FromQuery] string? name = null
    )
    {
        var cacheKey = $"Races_o{offset}_l{limit}_n{name}";

        if (memoryCache.TryGetValue<IEnumerable<Racer>>(cacheKey, out var raceDetails) && raceDetails is not null)
        {
            return Response.SetSuccess(raceDetails).ToResult();
        }

        var getResponse = await repository.GetRacersAsync(offset, limit, name);

        memoryCache.SetCache(cacheKey, getResponse.Data);

        return getResponse.ToResult();
    }
}
