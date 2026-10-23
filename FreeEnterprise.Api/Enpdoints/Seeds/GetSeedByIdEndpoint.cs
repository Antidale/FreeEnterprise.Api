using System.ComponentModel;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Seeds;

public static class GetSeedByIdEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetSeedById()
        {
            app.MapGet(ApiEndpoints.Seeds.GetById, async
            (
                [FromServices] ISeedRepository seedRepository,
                [Description("The specific SeedDetail to get, if you've previously stored the id.")]
                [FromRoute] int id
            ) =>
            {

                var response = await seedRepository.GetSeedByIdAsync(id);

                return response.ToResult();
            })
            .WithTags(OpenApiMetadata.Seeds.ToString())
            .WithOrder((int)OpenApiMetadata.Seeds)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound);
            return app;
        }
    }
}
