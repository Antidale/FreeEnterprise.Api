using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces
{
    public interface ITournamentRepository
    {
        Task<Response<int>> CreateTournamentAsync(CreateTournament createTournament);
        Task<Response<RegistrationPeriodChangeResponse>> UpdateRegistrationWindow(RegistrationPeriodStatusChange registrationPeriodStatusChange);
        Task<Response<RegistrationResponse>> RegisterPlayerAsync(Registration registration);
        Task<Response<RegistrationResponse>> DropPlayerAsync(Registration registration);
    }
}
