using System;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Tournaments;

public static class GetTournamentRegistrationsEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetTournamentRegistrations()
        {
            app.MapGet(ApiEndpoints.Tournament.GetTournamentRegistrations, GetTournamentRegistrantsAsync)
                .WithTags(OpenApiMetadata.TournamentAdmin.ToString())
                .WithOrder((int)OpenApiMetadata.TournamentAdmin);

            return app;
        }
    }

    public static async Task<IResult> GetTournamentRegistrantsAsync
    (
        [FromServices] IMemoryCache memoryCache,
        [FromServices] ITournamentRepository repository,
        [FromRoute] int id
    )
    {
        string cacheKey = $"Tournaments_{id}";
        if (memoryCache.TryGetValue(cacheKey, out List<TournamentRegistrant>? registrants) && registrants is not null)
        {
            return Classes.Response.SetSuccess(registrants).ToResult();
        }

        var response = await repository.GetTournamentRegistrantsAsync(id);
        memoryCache.Set(cacheKey, response.Data,
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
        return response.ToResult();

    }
}
