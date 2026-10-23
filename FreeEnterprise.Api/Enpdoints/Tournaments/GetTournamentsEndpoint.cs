using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;
namespace FreeEnterprise.Api.Enpdoints.Tournaments;

public static class GetTournamentsEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetTournamentSummaries()
        {
            app.MapGet(ApiEndpoints.Tournament.GetAll, GetTournamentSummariesAsync)
                .WithTags(OpenApiMetadata.TournamentAdmin.ToString())
                .WithOrder((int)OpenApiMetadata.TournamentAdmin)
                .WithDescription("Retieves all tournament summaries managed by the Tellah's Library discord bot. Results are cached for 30 minutes");

            return app;
        }
    }

    public static async Task<IResult> GetTournamentSummariesAsync
    (
        [FromServices]
        ITournamentRepository repository,
        [FromServices]
        IMemoryCache memoryCache
    )
    {
        const string cacheKey = "Tournaments";
        if (memoryCache.TryGetValue(cacheKey, out List<TournamentSummary>? tournaments) && tournaments is not null)
        {
            return Response.SetSuccess(tournaments).ToResult();
        }
        var response = await repository.GetTournamentSummariesAsync();

        memoryCache.SetCache(cacheKey, response.Data, TimeSpan.FromMinutes(30));

        return response.ToResult();
    }
}
