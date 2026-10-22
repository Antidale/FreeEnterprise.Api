using FeInfo.Common.Requests;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Entrants;

public static class EntrantsEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapEntrantEndpoints()
        {
            app.MapUpdateAlias();
            app.MapUpdatePronouns();
            app.MapUpdateTwitch();
            return app;
        }

        public IEndpointRouteBuilder MapUpdateTwitch()
        {
            app.MapPatch(ApiEndpoints.Entrants.UpdateTwitch, UpdateTwitchAsync)
                .WithTags("TournamentEntrants")
                .RequireAuthorization();
            return app;
        }

        public IEndpointRouteBuilder MapUpdatePronouns()
        {
            app.MapPatch(ApiEndpoints.Entrants.UpdatePronouns, UpdatePronounsAsync)
                .WithTags("TournamentEntrants")
                .RequireAuthorization();
            return app;
        }

        public IEndpointRouteBuilder MapUpdateAlias()
        {
            app.MapPatch(ApiEndpoints.Entrants.UpdateAlias, UpdateAliasAsync)
                .WithTags("TournamentEntrants")
                .RequireAuthorization();
            return app;
        }
    }

    private static async Task<IResult> UpdateTwitchAsync
    (
        [FromServices]
        IEntrantRepository repository,
        [FromBody]
        UpdateTwitch request
    )
    {
        var response = await repository.UpdateTwitchAsync(request);
        return response.GetResult();
    }

    private static async Task<IResult> UpdatePronounsAsync
    (
        [FromServices]
        IEntrantRepository repository,
        [FromBody]
        UpdatePronouns request
    )
    {
        var response = await repository.UpdatePronounsAsync(request);
        return response.GetResult();
    }

    private static async Task<IResult> UpdateAliasAsync
    (
        [FromServices]
        IEntrantRepository repository,
        [FromBody]
        UpdateAlias request
    )
    {
        var response = await repository.UpdateAliasAsync(request);
        return response.GetResult();
    }
}
