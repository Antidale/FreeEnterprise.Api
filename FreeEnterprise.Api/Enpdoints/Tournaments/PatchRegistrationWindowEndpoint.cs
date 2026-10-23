using FeInfo.Common.Requests;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Tournaments;

public static class PatchRegistrationWindowEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapUpdateRegistrationWindowEndpoint()
        {
            app.MapPatch(ApiEndpoints.Tournament.UpdateRegistrationWindow, async
            (
                [FromServices] ITournamentRepository repository,
                [FromBody] ChangeRegistrationPeriod request
            ) =>
            {
                var result = await repository.UpdateRegistrationWindow(request);
                return result.ToResult();
            })
            .WithTags(OpenApiMetadata.TournamentAdmin.ToString())
            .WithOrder((int)OpenApiMetadata.TournamentAdmin)
            .RequireAuthorization();
            return app;
        }
    }
}
