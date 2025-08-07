using System;

namespace FreeEnterprise.Api.Models;

public class RaceEntrant
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
    public int race_id { get; set; }
    public int entrant_id { get; set; }
    public TimeSpan? finish_time { get; set; }
    public int? placement { get; set; }
    public Dictionary<string, string> metadata { get; set; } = [];


#pragma warning restore IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
}
