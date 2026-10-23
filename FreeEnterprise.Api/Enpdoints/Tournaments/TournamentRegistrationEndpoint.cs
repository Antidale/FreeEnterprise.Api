using System;
using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Tournaments;

public static class TournamentRegistrationEndpoints
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapRegisterPlayerEndpoint()
        {
            app.MapPost(ApiEndpoints.Tournament.Register, async
            (
                [FromServices] ITournamentRepository repository,
                [FromBody] ChangeRegistration registration
            ) =>
            {
                var result = await repository.RegisterPlayerAsync(registration);
                return result.ToResult();
            });
            return app;
        }

        public IEndpointRouteBuilder MapDropPlayerEndpoint()
        {
            app.MapPost(ApiEndpoints.Tournament.Drop, async
            (
                [FromServices] ITournamentRepository repository,
                [FromBody] ChangeRegistration registration
            ) =>
            {
                var result = await repository.DropPlayerAsync(registration);
                return result.ToResult();
            });
            return app;
        }
    }


}
