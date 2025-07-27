using System;

namespace FreeEnterprise.Api.Classes;

public class RaceDetail
{
    public int RaceId { get; init; }
    public string RoomName { get; init; } = string.Empty;
    public string RaceType { get; init; } = string.Empty;
    public string RaceHost { get; init; } = string.Empty;
    public string Flagset { get; init; } = string.Empty;
    public int? SeedId { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = [];
}
