
namespace FreeEnterprise.Api.Enpdoints.BossInfo;

public static class BossInfoEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapBossInfoEndpoints()
        {
            app.MapGetBattleLocations();
            app.MapGetBossStats();
            app.MapGetBossBattles();
            return app;
        }
    }
}
