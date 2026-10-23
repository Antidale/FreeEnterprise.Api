using FeInfo.Common.Requests;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Tournaments;

public static class CreateTournamentEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapCreateTournamentEndpoint()
        {
            app.MapPost(ApiEndpoints.Tournament.Create, async
            (
                [FromServices] ITournamentRepository repository,
                [FromBody] CreateTournament request
            ) =>
            {
                var result = await repository.CreateTournamentAsync(request);
                return result.ToResult();
            })
            .WithTags(OpenApiMetadata.TournamentAdmin.ToString())
            .WithOrder((int)OpenApiMetadata.TournamentAdmin)
            .RequireAuthorization();

            return app;
        }
    }
}
