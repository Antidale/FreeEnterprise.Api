using System;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class RacersEndpointMapping
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapRacersEndpoints()
        {
            //add individual route mappings here
            app.MapGetRacersEndpoint();
            app.MapGetRacerByIdOrName();
            app.MapGetRacerRacesEndpoint();
            app.MapGetHeadToHeadEndpoint();
            app.MapSearchRacetimeUser();
            return app;
        }
    }
}
