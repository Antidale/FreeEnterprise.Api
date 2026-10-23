using System;
using System.ComponentModel;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Seeds;

public static class GetSeedHtmlEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetSeedHtml()
        {
            app.MapGet(ApiEndpoints.Seeds.GetSeedHtml, async
            (
                [FromServices]
                ISeedRepository repository,
                [Description($"The id of the specific seed to get the patch page for, if you've previously stored the id. If you only know the race room, prefer using {ApiEndpoints.Races.GetRaceSeed}.")]
                [FromRoute]int id
            ) =>
            {
                var response = await repository.GetPatchBySeedIdAsync(id);

                return response.ToResult();
            })
            .WithTags(OpenApiMetadata.Seeds.ToString())
            .WithOrder((int)OpenApiMetadata.Seeds)
            .WithDescription("Returns the actual page page html as a string. suitable for using in an iFrame, or for saving to your own system.");


            return app;
        }
    }
}
