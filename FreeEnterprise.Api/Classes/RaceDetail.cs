using System;

namespace FreeEnterprise.Api.Classes;

public class RaceDetail
{
    public string RoomName { get; init; } = string.Empty;
    public string RaceType { get; init; } = string.Empty;
    public string RaceHost { get; init; } = string.Empty;
    public Dictionary<string, string> Metadata { get; init; } = [];
}
