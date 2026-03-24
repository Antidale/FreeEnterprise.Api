using System.Text.Json.Serialization;

namespace FreeEnterprise.Api.RtggModels;

public record class Entrant
{
    public required User User { get; set; }
    public Team? Team { get; set; }
    public required Status Status { get; set; }
    [JsonPropertyName("finish_time")]
    public string? FinishTime { get; set; }
    public int? Place { get; set; }
    public int? Score { get; set; }
    [JsonPropertyName("score_change")]
    public int? ScoreChange { get; set; }
    public string? Comment { get; set; }
    [JsonPropertyName("has_comment")]
    public bool HasComment { get; set; }
    [JsonPropertyName("stream_live")]
    public bool StreamLive { get; set; }
    [JsonPropertyName("stream_overrite")]
    public bool StreamOverride { get; set; }

    public Models.CreateRaceEntrantModel ToRaceEntrant(string roomName)
    {
        var metaData = new Dictionary<string, string>();
        if (Score.HasValue)
            metaData.Add("score", Score!.Value.ToString());

        if (ScoreChange.HasValue)
            metaData.Add("scoreChange", ScoreChange!.Value.ToString());

        if (!string.IsNullOrWhiteSpace(Comment))
            metaData.Add("comment", Comment);

        if (!Status.Value.Equals("done", StringComparison.InvariantCultureIgnoreCase))
            metaData.Add("status", Status.Value);

        return new Models.CreateRaceEntrantModel
        {
            finish_time = FinishTime,
            metadata = metaData,
            placement = Place,
            racetime_id = User.Id,
            room_name = roomName
        };
    }
}
