using System;
using System.ComponentModel;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Races;

public static class GetRaceSeedEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetRaceSeedEndpoint()
        {
            app.MapGet(ApiEndpoints.Races.GetRaceSeed, async
            (
                [FromServices] IRaceRepository repository,
                [Description("The internal id (int)for the race, or the name of the racetime room.")]
                [FromRoute] string idOrSlug
            ) =>
            {
                var raceResponse = await repository.GetRaceSeedHtmlAsync(idOrSlug);
                return raceResponse.ToContentResult();
            })
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription("Returns the entire html document to reconstruct the patch page, suitable for putting into an iframe")
            .WithTags(OpenApiMetadata.Races.ToString())
            .WithOrder((int)OpenApiMetadata.Races);

            return app;
        }
    }
}
