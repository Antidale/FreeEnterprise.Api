using System;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Equipment;

public static class GetWeaponsEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetWeapons()
        {
            app.MapGet(ApiEndpoints.Equipment.GetAllWeapons, async
            (
                [FromServices]
                IEquipmentRepository repository
            ) =>
            {
                var equipment = await repository.GetWeaponsAsync();
                return TypedResults.Ok(equipment);
            })
            .WithTags(OpenApiMetadata.Equipment.ToString())
            .WithOrder((int)OpenApiMetadata.Equipment);

            return app;
        }
    }
}
