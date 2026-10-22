using System;

namespace FreeEnterprise.Api.Enpdoints.Races;

public static class RacesEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapRacesEndpoints()
        {
            app.MapCreateRace()
               .MapGetRaces();
            return app;
        }
    }
}
