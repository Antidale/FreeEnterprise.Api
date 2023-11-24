using FreeEnterprise.Api.DTOs;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        public Task CloseRegistrationAsync()
        {
            // set nameof(CreateTournamentRequest.RegistrationEnd) to utc now
            throw new NotImplementedException();
        }

        public Task CreateTournamentAsync()
        {
            throw new NotImplementedException();
        }

        public Task OpenRegistrationAsync()
        {
            // set nameof(CreateTournamentRequest.RegistrationStart) to utc now
            throw new NotImplementedException();
        }

        public Task ResigterPlayerAsync()
        {
            // add player if not exists
            // add registration for tournament
            /*
                return roleid/name, and information to update tracking message back to bot
            */
            throw new NotImplementedException();
        }

        public Task UnregisterPlayerAsync()
        {
            // remove registration for tournament
            /*
                return roleid/name, and information to update tracking message back to bot
            */
            throw new NotImplementedException();
        }
    }
}