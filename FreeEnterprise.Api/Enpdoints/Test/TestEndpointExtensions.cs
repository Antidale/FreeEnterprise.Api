using System;

namespace FreeEnterprise.Api.Enpdoints.Test;

public static class TestEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapTestEndpoints()
        {
            app.MapGetTests();
            return app;
        }
    }
}
