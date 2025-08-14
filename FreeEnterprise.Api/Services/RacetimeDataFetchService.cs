using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Rtgg = FreeEnterprise.Api.RtggModels;

namespace FreeEnterprise.Api.Services;

public class RacetimeDataFetchService(
    ILogger<RacetimeDataFetchService> logger,
    IHttpClientFactory httpClientFactory,
    IRaceRespository raceRespository,
    IRacerRepository racerRepository
) : BackgroundService
{
    private readonly ILogger<RacetimeDataFetchService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timeSpan = TimeSpan.FromHours(1);


#if DEBUG
        timeSpan = TimeSpan.FromMinutes(1);
#endif

        using PeriodicTimer timer = new(timeSpan);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("error: {ex}", ex.Message);
        }
    }

    private async Task DoWork()
    {
        /*
            With the frequencey this runs (hourly) and the frequence that races happen (very infrequently), 20 is probably too large a number, but it is also likely safe
        */

        var rtggRaces = new List<Rtgg.Race>();
        try
        {
            rtggRaces = await GetRecentRecordedRtggRaces();

            if (rtggRaces.Count == 0)
            {
                _logger.LogInformation("No recent races found");
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception getting races from racetime: {ex}", ex.ToString());
            return;
        }

        var dbRaces = await raceRespository.GetRacesAsync(0, 60, true, null, null);

        //Eliminate any races that have already had an EndedAt set, which means that they've already been updated through this. Do have to add back in the game prefix, since I strip that out in this database
        var recentdbRaceRooms = dbRaces.Data?.Where(x => x.EndedAt is not null).Select(x => $"ff4fe/{x.RoomName}") ?? [];

        rtggRaces = [.. rtggRaces.Where(x => !recentdbRaceRooms.Contains(x.Name))];

        var racers = rtggRaces.SelectMany(x => x.Entrants.Select(x => x.User)).Distinct().Select(x => new Racer
        {
            racetime_display_name = x.Name,
            racetime_id = x.Id,
            twitch_name = x.TwitchDisplayName
        }).ToList();

        //upsert runner
        await racerRepository.MergeRacersAsync(racers);

        //upsert race
        var races = rtggRaces.Select(x => new Race
        {
            race_type = "FFA",
            room_name = x.Name,
            race_host = "Racetime.gg",
            //a recorded race should have an Ended at, and we're pulling only recorded races to this point
            ended_at = x.EndedAt ?? DateTime.UtcNow,
            metadata = new Dictionary<string, string>
            {
                ["Goal"] = x.Goal.Name,
                ["Description"] = x.Info,
                ["Entrants"] = x.EntrantsCount.ToString(),
                ["Status"] = x.Status.Value
            }
        });


        //Insert race_entrants
    }

    private async Task<List<Rtgg.Race>> GetRecentRecordedRtggRaces()
    {
        _logger.LogInformation("Doing stuff");
        var client = httpClientFactory.CreateClient();
        try
        {
            var recentRacesResponse = await client.GetAsync("https://racetime.gg/ff4fe/races/data?page=1&per_page=20&show_entrants=1");
            recentRacesResponse.EnsureSuccessStatusCode();
            var racesResponse = await recentRacesResponse.Content.ReadFromJsonAsync<Rtgg.RacesResponse>() ?? new();
            return [.. racesResponse.Races.Where(x => x.Recorded)];
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed in updating data from racetime: {ex}", ex.Message);
            return [];
        }
    }
}
