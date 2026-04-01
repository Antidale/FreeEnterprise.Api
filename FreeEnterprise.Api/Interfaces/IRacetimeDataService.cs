using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.RtggModels;

namespace FreeEnterprise.Api.Interfaces;

public interface IRacetimeDataService
{
    /// <summary>
    /// Queries racetime.gg for the races in the ff4fe category, and filters out ones that are not recorded.
    /// </summary>
    /// <param name="page">how many 'pages' back to request. defaults to the starting page</param>
    /// <param name="perPage">how many results to return in this request, defaults to 100</param>
    /// <returns>A list of Race objects in the recorded state, including the RaceEntrants for those races.</returns>
    Task<Response<List<Race>>> GetRecentRecordedRtggRaces(int page = 1, int perPage = 100);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="querystring"></param>
    /// <returns></returns>
    Task<Response<string>> SearchRacetimeUserAsync(string querystring);
}
