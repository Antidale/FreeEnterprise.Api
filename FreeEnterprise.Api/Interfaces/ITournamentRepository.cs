using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces
{
    public interface ITournamentRepository
    {
        Task CreateTournamentAsync();
        Task OpenRegistrationAsync();
        Task CloseRegistrationAsync();
        Task ResigterPlayerAsync();
        Task UnregisterPlayerAsync();
    }
}
