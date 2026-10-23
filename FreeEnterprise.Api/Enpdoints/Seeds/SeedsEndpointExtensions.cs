using System;

namespace FreeEnterprise.Api.Enpdoints.Seeds;

public static class SeedsEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapSeedsEndpoints()
        {
            app.MapGetSeedHtml();
            app.MapGetSeedById();
            app.MapLogSeedEndpoint();
            app.MapGetSeedsEndpoint();
            return app;
        }
    }
}
