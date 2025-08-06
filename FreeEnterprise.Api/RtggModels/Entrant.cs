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
}
