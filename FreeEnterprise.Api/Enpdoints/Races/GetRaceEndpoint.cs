using System;
using System.ComponentModel;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Races;

public static class GetRaceEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetRaceEndpoint()
        {
            app.MapGet(ApiEndpoints.Races.Get, async
            (
                [FromServices] IRaceRepository repository,
                [Description("The internal id (int)for the race, or the name of the racetime room.")]
                [FromRoute] string idOrSlug
            ) =>
            {
                var raceResponse = await repository.GetRaceAsync(idOrSlug);
                return raceResponse.ToResult();
            })
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription("The detail for the specifc race, including entrants")
            .WithTags(OpenApiMetadata.Races.ToString())
            .WithOrder((int)OpenApiMetadata.Races);

            return app;
        }
    }
}
