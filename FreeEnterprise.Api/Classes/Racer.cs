namespace FreeEnterprise.Api.Classes;

public record Racer
{
    public required string RacetimeName { get; init; }
    public required string TwitchName { get; init; }
    public required string RacetimeId { get; init; }
    public string RacetimeProfile => string.IsNullOrWhiteSpace(RacetimeId)
        ? string.Empty
        : $"https://racetime.gg/user/{RacetimeId}";
}
