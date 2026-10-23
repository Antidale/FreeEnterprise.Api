using System.ComponentModel;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class GetRacerByIdOrNameEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetRacerByIdOrName()
        {
            app.MapGet(ApiEndpoints.Racers.GetByIdOrName, async
            (
                [FromServices]
                IRacerRepository repository,
                [Description("preferably, use the user's id (not name) from racetime.gg. The user's name at racetime and an associated twitch account can match, but might result in a 409 if multiple different users share that information")]
                [FromRoute]
                string idOrName
            ) =>
            {
                var getResponse = await repository.GetRacerAsync(idOrName);
                return getResponse.ToResult();
            })
            .WithTags(OpenApiMetadata.Racers.ToString())
            .WithOrder((int)OpenApiMetadata.Racers)
            .WithDescription("Returns a Racer object for the given user, if found.")
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status404NotFound);

            return app;
        }
    }

}
