using System.Text.Json.Serialization;

namespace FreeEnterprise.Api.RtggModels;

public record class Status
{
    public string Value { get; set; } = string.Empty;
    [JsonPropertyName("verbose_value")]
    public string VerboseValue { get; set; } = string.Empty;
    [JsonPropertyName("help_text")]
    public string HelpText { get; set; } = string.Empty;
}
