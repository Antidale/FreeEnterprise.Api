using System.ComponentModel;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class GetRacerHeadToHeadEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetHeadToHeadEndpoint()
        {
            app.MapGet(ApiEndpoints.Racers.CompareRacers, GetRacerHeadToHeadAsync)
                .WithTags(OpenApiMetadata.Racers.ToString())
                .WithOrder((int)OpenApiMetadata.Racers)
                .WithDescription("Provides all of the races both runners have raced in");
            return app;
        }
    }

    public static async Task<IResult> GetRacerHeadToHeadAsync
    (
        [FromServices] IMemoryCache memoryCache,
        [FromServices] IRacerRepository repository,
        [Description("Either the ractime.gg id, or the racer's display name for either rt.gg or twitch")]
        [FromRoute] string idOrName,
        [Description("Either the ractime.gg id, or the racer's display name for either rt.gg or twitch")]
        [FromRoute] string opponentIdOrName
    )
    {
        var cacheKey = $"Racer_h2h_n{idOrName}_opp{opponentIdOrName}";

        if (memoryCache.TryGetValue<IEnumerable<Racer>>(cacheKey, out var raceDetails) && raceDetails is not null)
        {
            return Response.SetSuccess(raceDetails).ToResult();
        }

        var response = await repository.GetHeadToHeadAsync(idOrName, opponentIdOrName);

        memoryCache.SetCache(cacheKey, response.Data);

        return response.ToResult();
    }
}
