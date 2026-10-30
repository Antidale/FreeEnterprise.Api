using FreeEnterprise.Api.Enpdoints.BossInfo;
using FreeEnterprise.Api.Enpdoints.Entrants;
using FreeEnterprise.Api.Enpdoints.Equipment;
using FreeEnterprise.Api.Enpdoints.Guides;
using FreeEnterprise.Api.Enpdoints.Racers;
using FreeEnterprise.Api.Enpdoints.Races;
using FreeEnterprise.Api.Enpdoints.Seeds;
using FreeEnterprise.Api.Enpdoints.Test;
using FreeEnterprise.Api.Enpdoints.Tournaments;

namespace FreeEnterprise.Api.Enpdoints;

public static class EndpointMapping
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapEndpoints()
        {
            app
               .MapBossInfoEndpoints()
               .MapEntrantEndpoints()
               .MapEquipmentEnpoints()
               .MapGuideEndpoints()
               .MapRacesEndpoints()
               .MapRacersEndpoints()
               .MapSeedsEndpoints()
               .MapTournamentEndpoints();

#if DEBUG
            app.MapTestEndpoints();
#endif

            return app;
        }
    }
}
