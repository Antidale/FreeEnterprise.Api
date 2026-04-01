using System.Text.Json.Serialization;

namespace FreeEnterprise.Api.RtggModels;

public record class Race
{
    public required string Name { get; set; }
    public required Status Status { get; set; }
    public required string Url { get; set; }

    [JsonPropertyName("data_url")]
    public required string DataUrl { get; set; }
    public required Goal Goal { get; set; }
    public required string Info { get; set; }

    [JsonPropertyName("entrants_count")]
    public int EntrantsCount { get; set; }

    [JsonPropertyName("entrants_count_finished")]
    public int EntrantsCountFinished { get; set; }

    [JsonPropertyName("entrants_count_inactive")]
    public int EntrantsCountInactive { get; set; }

    [JsonPropertyName("opened_at")]
    public DateTime OpenedAt { get; set; }

    [JsonPropertyName("started_at")]
    public DateTime? StartedAt { get; set; }

    [JsonPropertyName("time_limit")]
    public required string TimeLimit { get; set; }

    [JsonPropertyName("opened_by_bot")]
    public string? OpenedByBot { get; set; }
    public List<Entrant> Entrants { get; set; } = [];

    [JsonPropertyName("ended_at")]
    public DateTime? EndedAt { get; set; }

    [JsonPropertyName("cancelled_at")]
    public DateTime? CancelledAt { get; set; }
    public bool Recordable { get; set; }
    public bool Recorded { get; set; }

    public string DbName => Name.Split('/').Last();

    public Models.Race ToRaceModel()
    {

        //strip out links, at least. the hash can stay? Also, 
        var info = string.Join(" ", Info.ReplaceLineEndings().Split(Environment.NewLine).Where(x => !x.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)));

        return new Models.Race
        {
            race_type = "FFA",
            room_name = DbName,
            race_host = "Racetime.gg",
            //a recorded race should have an Ended at, and we're pulling only recorded races to this point, so if for some reason RT.gg has borked, we'll just put something there, since we're using the presence of data in this field as the indication that the race is complete
            ended_at = EndedAt ?? DateTime.UtcNow,
            metadata = new Dictionary<string, string>
            {
                ["Goal"] = Goal.Name,
                //Just take the first newline
                ["Description"] = info,
                ["Entrants"] = EntrantsCount.ToString(),
                ["Status"] = Status.Value
            }
        };
    }

    public List<Models.CreateRaceEntrantModel> ToCreateEntrantModels()
    {
        return [.. Entrants.Select(x => x.ToRaceEntrant(roomName: DbName))];
    }
}
