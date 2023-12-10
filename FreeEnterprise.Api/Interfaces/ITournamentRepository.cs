using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces
{
    public interface ITournamentRepository
    {
        Task<Response<int>> CreateTournamentAsync(CreateTournament createTournament);
        Task<Response<ChangeRegistrationPeriodResponse>> UpdateRegistrationWindow(ChangeRegistrationPeriod registrationPeriodStatusChange);
        Task<Response<ChangeRegistrationResponse>> RegisterPlayerAsync(ChangeRegistration registration);
        Task<Response<ChangeRegistrationResponse>> DropPlayerAsync(ChangeRegistration registration);
        Task<Response<List<TournamentSummary>>> GetTournamentSummariesAsync();

    }
}
