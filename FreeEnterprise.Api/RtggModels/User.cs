using System.Text.Json.Serialization;

namespace FreeEnterprise.Api.RtggModels;

public record User()
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    public string Discriminator { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Pronouns { get; set; } = string.Empty;
    public string Flair { get; set; } = string.Empty;
    [JsonPropertyName("twitch_name")]
    public string TwitchName { get; set; } = string.Empty;
    [JsonPropertyName("twitch_display_name")]
    public string TwitchDisplayName { get; set; } = string.Empty;
    [JsonPropertyName("twitch_thannel")]
    public string TwitchChannel { get; set; } = string.Empty;
    [JsonPropertyName("can_moderate")]
    public bool CanModerate { get; set; }
}
