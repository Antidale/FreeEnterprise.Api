using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Equipment;

public static class GetArmorByIdEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetArmorByIdAsync()
        {
            app.MapGet(ApiEndpoints.Equipment.GetArmorById, async
            (
                [FromServices]
                IEquipmentRepository repository,
                [FromRoute]
                int id
            ) =>
            {
                var equipment = await repository.GetArmorAsync(id);
                return TypedResults.Ok(equipment);
            })
            .WithTags(OpenApiMetadata.Equipment.ToString())
            .WithOrder((int)OpenApiMetadata.Equipment);

            return app;
        }
    }
}
