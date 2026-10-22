using System;

namespace FreeEnterprise.Api.Enpdoints.Guides;

public static class GuideEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGuideEndpoints()
        {
            app.MapGuideSearch()
               .MapGetBossStrategy();

            return app;
        }
    }
}
