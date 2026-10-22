using System;

namespace FreeEnterprise.Api.Enpdoints.Racers;

public static class RacersEndpointMapping
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapRacersEndpoints()
        {
            //add individual route mappings here

            return app;
        }
    }
}
