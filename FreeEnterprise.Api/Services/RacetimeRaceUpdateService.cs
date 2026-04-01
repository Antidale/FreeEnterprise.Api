using FreeEnterprise.Api.Interfaces;
using Rtgg = FreeEnterprise.Api.RtggModels;

namespace FreeEnterprise.Api.Services;

public class RacetimeRaceUpdateService(
    ILogger<RacetimeRaceUpdateService> logger,
    IRacetimeDataService racetimeDataService,
    IRaceRespository raceRespository,
    IRacerRepository racerRepository,
    IRaceEntrantRepository raceEntrantRepository
) : BackgroundService
{
    private readonly ILogger<RacetimeRaceUpdateService> _logger = logger;

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
                await UpdateRtggRaceData();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("error: {ex}", ex.Message);
        }
    }

    private async Task UpdateRtggRaceData()
    {
        var rtggRaces = await GetRtggRacesAsync();

        var racers = rtggRaces.SelectMany(x => x.Entrants.Select(x => x.User)).Distinct().Select(x => new Models.Racer
        {
            racetime_display_name = x.Name,
            racetime_id = x.Id,
            twitch_name = x.TwitchDisplayName
        }).ToList();

        //upsert runner
        await racerRepository.MergeRacersAsync(racers);

        //merge races
        var races = rtggRaces.Select(x => x.ToRaceModel()).ToList();
        var raceMergeResponse = await raceRespository.MergeRacesAsync(races);
        if (!raceMergeResponse.Success)
        {
            _logger.LogError("Error in merging races from rt.gg into database");
        }

        var entrants = rtggRaces.SelectMany(x => x.ToCreateEntrantModels()).ToList();

        //Insert race_entrants
        await raceEntrantRepository.InsertRaceEntrants(entrants);
    }

    public async Task<List<Rtgg.Race>> GetRtggRacesAsync()
    {
        try
        {
            //method defaults to 10 races, will have to figure out later if we care about changing that. After the first time through, it can probably go down to like 10 or 20
            var getResponse = await racetimeDataService.GetRecentRecordedRtggRaces();

            if (!getResponse.Success || getResponse.Data is null)
            {
                _logger.LogError("Issue getting races from racetime.");
                return [];
            }

            if (getResponse.Data.Count == 0)
            {
                _logger.LogInformation("No recent races found");
                return [];
            }

            var rtggRaces = getResponse.Data;

            var dbRaces = await raceRespository.GetEndedRacetimeRoomNames();
            if (!dbRaces.Success | dbRaces.Data is null)
            {
                _logger.LogError("Error fetching races from db. Aborting this attempt");
                return [];
            }

            return [.. rtggRaces.Where(x => !dbRaces.Data!.Contains(x.Name))];
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception getting races from racetime: {ex}", ex.ToString());
            return [];
        }
    }
}
