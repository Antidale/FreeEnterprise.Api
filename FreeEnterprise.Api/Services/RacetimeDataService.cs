using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.RtggModels;

namespace FreeEnterprise.Api.Services;

public class RacetimeDataService(IHttpClientFactory httpClientFactory, ILogger<RacetimeDataService> logger) : IRacetimeDataService
{
    public async Task<Response<List<Race>>> GetRecentRecordedRtggRaces(int page = 1, int perPage = 30)
    {
        logger.LogInformation("Fetching Races");
        var client = httpClientFactory.CreateClient();
        try
        {
            //TODO: Consider handling other hosts, for various running locally/testing scenarios
            var url = $"https://racetime.gg/ff4fe/races/data?page={page}&per_page={perPage}&show_entrants=1";
            var recentRacesResponse = await client.GetAsync(url);
            recentRacesResponse.EnsureSuccessStatusCode();

            var racesResponse = await recentRacesResponse.Content.ReadFromJsonAsync<RacesResponse>() ?? new();

            return Response.SetSuccess<List<Race>>([.. racesResponse.Races.Where(x => x.Recorded)]);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed in updating data from racetime: {ex}", ex.Message);
            return Response.BadRequest<List<Race>>(ex.Message);
        }
    }

    public async Task<Response<string>> SearchRacetimeUserAsync(string querystring)
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://racetime.gg/user/search{querystring}");
        try
        {
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return Response.SetSuccess(result);
        }
        catch (Exception ex)
        {
            return Response.BadRequest<string>(ex.Message);
        }


    }
}
