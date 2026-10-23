using System;

namespace FreeEnterprise.Api.Enpdoints.Tournaments;

public static class TournamentEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapTournamentEndpoints()
        {
            app.MapGetTournamentSummaries();
            app.MapGetTournamentRegistrations();
            app.MapCreateTournamentEndpoint();
            app.MapRegisterPlayerEndpoint();
            app.MapDropPlayerEndpoint();
            app.MapUpdateRegistrationWindowEndpoint();
            return app;
        }
    }
}
