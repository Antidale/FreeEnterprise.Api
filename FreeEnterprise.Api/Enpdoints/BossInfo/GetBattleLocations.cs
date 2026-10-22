using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.BossInfo;

public static class BattleLocationEndpoints
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetBattleLocations()
        {
            app.MapGet(ApiEndpoints.BattleLocations.GetAll, async (
                [FromServices]
                IBattleLocationsRepository repository
            ) =>
            {
                var locations = await repository.GetBattleLocationsAsync();
                return TypedResults.Ok(locations);
            })
            .WithTags(OpenApiMetadata.BossInfo.ToString())
            .WithOrder((int)OpenApiMetadata.BossInfo)
            .WithDescription("some description");

            return app;
        }
    }
}
