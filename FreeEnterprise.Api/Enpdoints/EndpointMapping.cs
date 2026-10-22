using FreeEnterprise.Api.Enpdoints.BossInfo;
using FreeEnterprise.Api.Enpdoints.Entrants;
using FreeEnterprise.Api.Enpdoints.Equipment;
using FreeEnterprise.Api.Enpdoints.Guides;
using FreeEnterprise.Api.Enpdoints.Racers;
using FreeEnterprise.Api.Enpdoints.Races;
using FreeEnterprise.Api.Enpdoints.Test;

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
               .MapTestEndpoints()
               .MapGuideEndpoints()
               .MapRacesEndpoints()
               .MapRacersEndpoints();

            return app;
        }
    }
}
