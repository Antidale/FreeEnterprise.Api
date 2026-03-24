using System;

namespace FreeEnterprise.Api.Models;

public class CreateRaceEntrantModel
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
    public required string room_name { get; set; }
    public required string racetime_id { get; set; }
    public string? finish_time { get; set; }
    public int? placement { get; set; }
    public Dictionary<string, string> metadata { get; set; } = [];


#pragma warning restore IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
}
