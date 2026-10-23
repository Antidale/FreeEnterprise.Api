using System;
using System.ComponentModel;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class GetRacerRacesEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetRacerRacesEndpoint()
        {
            app.MapGet(ApiEndpoints.Racers.GetRacerRaces, GetRacerRacesAsync)
                .WithTags(OpenApiMetadata.Racers.ToString())
                .WithOrder((int)OpenApiMetadata.Racers)
                .ProducesValidationProblem();
            return app;
        }
    }

    public static async Task<IResult> GetRacerRacesAsync
    (
        [FromServices] IRacerRepository repository,
        [FromServices] IMemoryCache memoryCache,
        [Description("the racetime id of the user, or their ractime mae or the name of the twitch account registered to their racetime account")]
        [FromRoute] string idOrName,
        [FromQuery] int offset = 0,
        [Description("The amount of RaceDetails to return in this call. Combine with offset to page through records")]
        [FromQuery] int limit = 20
    )
    {
        if (limit > 300)
        {
            return Results.ValidationProblem
            (
                [new(nameof(limit), ["Cannot request more than 300 races"])],
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var cacheKey = $"Racer_n{idOrName}_o{offset}_l{limit}";

        if (memoryCache.TryGetValue<IEnumerable<RaceDetail>>(cacheKey, out var raceDetails) && raceDetails is not null)
        {
            return Classes.Response.SetSuccess(raceDetails).ToResult();
        }

        var response = await repository.GetRacesForRacerAsync(idOrName, offset, limit);

        memoryCache.SetCache(cacheKey, response.Data);

        return response.ToResult();
    }
}
