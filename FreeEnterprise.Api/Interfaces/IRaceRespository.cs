using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces;

public interface IRaceRespository
{
    Task<Response<int>> CreateRaceAsync(CreateRaceRoom createRequest);
    Task<Response<IEnumerable<RaceDetail>>> GetRacesAsync(int offset, int limit, bool includeCancelled, string? description, string? flagset);
    Task<Response<RaceDetail>> GetRaceAsync(string idOrSlug);
    Task<Response<string>> GetRaceSeedHtmlAsync(string idOrSlug);
    // Task<Response> JoinRaceAsync(JoinRaceRequest joinRaceRequest);
}
