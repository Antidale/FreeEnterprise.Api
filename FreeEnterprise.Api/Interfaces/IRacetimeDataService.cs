using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.RtggModels;

namespace FreeEnterprise.Api.Interfaces;

public interface IRacetimeDataService
{
    Task<Response<List<Race>>> GetRecentRecordedRtggRaces(int page = 1, int perPage = 30);
    Task<Response<string>> SearchRacetimeUserAsync(string querystring);
}
