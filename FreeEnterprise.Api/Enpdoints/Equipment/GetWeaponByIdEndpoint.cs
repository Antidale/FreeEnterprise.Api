using System;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Equipment;

public static class GetWeaponByIdEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetWeaponAsync(
        )
        {
            app.MapGet(ApiEndpoints.Equipment.GetWeaponById, async
            (
                [FromServices]
                IEquipmentRepository repository,
                [FromRoute]
                int id
            ) =>
            {
                var equipment = await repository.GetWeaponAsync(id);
                return TypedResults.Ok(equipment);
            })
            .WithTags(OpenApiMetadata.Equipment.ToString())
            .WithOrder((int)OpenApiMetadata.Equipment);

            return app;
        }
    }
}
