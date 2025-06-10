

// ReSharper disable InconsistentNaming
using System.Text.Json;
using System.Text.Json.Serialization;
using FeInfo.Common.Requests;

namespace FreeEnterprise.Api.Models;

public class Race
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database

    public int id { get; init; }
    public string room_name { get; init; } = string.Empty;
    public string race_type { get; init; } = string.Empty;
    public string race_host { get; init; } = string.Empty;
    public Dictionary<string, string> metadata { get; init; } = [];

    public Race() { }
    public Race(CreateRaceRoom createRaceRoom)
    {
        id = 0;
        room_name = createRaceRoom.RoomName;
        race_host = createRaceRoom.RaceHost;
        race_type = createRaceRoom.RaceType;
        metadata = createRaceRoom.Metadata;
    }


#pragma warning restore IDE1006 // Naming Styles
}
