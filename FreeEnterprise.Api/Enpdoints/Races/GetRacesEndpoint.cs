using System.ComponentModel;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Races;

public static class GetRacesEndpoint
{
    public static IEndpointRouteBuilder MapGetRaces(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Races.GetAll, GetRacesAsync)
            .WithTags(OpenApiMetadata.Races.ToString())
            .WithOrder((int)OpenApiMetadata.Races)
            .WithDescription("Returns a collection of RaceDetail objects. Unrecorded races are returned first, then in descending order of the race's finish time")
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> GetRacesAsync(
        IRaceRepository repository,
        IMemoryCache memoryCache,
        [Description("The amount of records to skip. Combined with limit, lets you page through results")]
        int offset = 0,
        [Description("The amount of records to return. Max 500.")]
        int limit = 20,
        [Description("By default, races with a status of 'cancelled' are excluded. set to true to also return these")]
        bool includeCancelled = false,
        [Description("A word or phrase used in the race's description. If you're looking for races for a specific racer, use the /racers endoints instead.")]
        string? description = null,
        [Description("A portion of the flagset you want to search for. There are some oddities to this.")]
        string? flagset = null)
    {
        if (limit > 500)
        {
            return Results.Problem("Cannot ask for more than 500 races", statusCode: StatusCodes.Status400BadRequest);
        }

        //Super quick and dirty to get eveything working again very quickly. Address in a better way tomorrow.
        if (flagset == string.Empty)
        {
            flagset = null;
        }

        if (description == string.Empty)
        {
            description = null;
        }

        var cacheKey = $"Races_o{offset}_l{limit}_d{description}_f{flagset}_{includeCancelled}";

        if (memoryCache.TryGetValue<IEnumerable<RaceDetail>>(cacheKey, out var raceDetails) && raceDetails is not null)
        {
            return Response.SetSuccess(raceDetails).ToResult();
        }

        var getResponse = await repository.GetRacesAsync(offset, limit, includeCancelled, description, flagset);

        memoryCache.SetCache(cacheKey, getResponse.Data);
        return getResponse.ToResult();
    }
}
