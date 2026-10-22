using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.BossInfo;

public static class GetBossBattlesEnpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetBossBattles()
        {
            app.MapGet(ApiEndpoints.BossBattle.GetAll, async (
                [FromServices]
                IBossBattlesRepository repository
            ) =>
            {
                var locations = await repository.GetBossBattlesAsync();
                return TypedResults.Ok(locations);
            })
            .WithTags("BossInfo");

            return app;
        }
    }
}
