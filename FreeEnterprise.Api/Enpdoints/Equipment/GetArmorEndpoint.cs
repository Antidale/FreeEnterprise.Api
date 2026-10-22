using System;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Equipment;

public static class GetArmorEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetArmor()
        {
            app.MapGet(ApiEndpoints.Equipment.GetAllArmor, async
            (
                [FromServices]
                IEquipmentRepository repository
            ) =>
            {
                var equipment = await repository.GetArmorAsync();
                return TypedResults.Ok(equipment);
            })
            .WithTags(OpenApiMetadata.Equipment.ToString())
            .WithOrder((int)OpenApiMetadata.Equipment);

            return app;
        }
    }
}
