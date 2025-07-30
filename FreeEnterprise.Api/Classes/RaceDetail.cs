using System;

namespace FreeEnterprise.Api.Classes;

public record RaceDetail()
{
    public int RaceId { get; init; }
    public string RoomName { get; init; } = string.Empty;
    public string RaceType { get; init; } = string.Empty;
    public string RaceHost { get; init; } = string.Empty;
    public string Flagset { get; init; } = string.Empty;
    public int? SeedId { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = [];

    /// <summary>
    /// Returns a new instance of this class, but applying a filter to remove metadata entries that start with the given string filter
    /// </summary>
    /// <param name="keyFilter">The string to apply as a filter</param>
    /// <returns></returns>
    public RaceDetail WithFilteredMetadata(string keyFilter) => this with
    {
        Metadata = Metadata.Where(x => !x.Key.StartsWith(keyFilter)).ToDictionary()
    };
}
