using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces;

public interface IRaceRespository
{
    Task<Response<int>> CreateRaceAsync(CreateRaceRoom createRequest);
    Task<Response<IEnumerable<RaceDetail>>> GetRacesAsync(int offset, int limit, bool includeCancelled, string? description, string? flagset);
    Task<Response<RaceDetail>> GetRaceAsync(string idOrSlug);
    Task<Response<string>> GetRaceSeedHtmlAsync(string idOrSlug);
    Task<Response> MergeRacesAsync(List<Race> races);
    /// <summary>
    /// Finds all races with a host of "Racetime.gg" and a non null entrance type
    /// </summary>
    /// <returns>the room name as rt.gg returns from their api, so there is a ff4fe/ prepended to what is in the database</returns>
    Task<Response<List<string>>> GetEndedRacetimeRoomNames();

    // Task<Response> JoinRaceAsync(JoinRaceRequest joinRaceRequest);
}
