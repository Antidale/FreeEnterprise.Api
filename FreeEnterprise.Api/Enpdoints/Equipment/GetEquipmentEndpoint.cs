using System;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Equipment;

public static class GetEquipmentEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetEquipment()
        {
            app.MapGet(ApiEndpoints.Equipment.GetAll, async
            (
                [FromServices]
                IEquipmentRepository repository
            ) =>
            {
                var equipment = await repository.GetEquipmentAsync();
                return TypedResults.Ok(equipment);
            })
            .WithTags(OpenApiMetadata.Equipment.ToString())
            .WithOrder((int)OpenApiMetadata.Equipment);

            return app;
        }
    }

}
