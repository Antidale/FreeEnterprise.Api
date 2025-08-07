using System;

namespace FreeEnterprise.Api.Models;

public class Racer
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
    public int id { get; set; }
    public string discord_id { get; set; } = string.Empty;
    public string discord_display_name { get; set; } = string.Empty;
    public string racetime_id { get; set; } = string.Empty;
    public string twitch_name { get; set; } = string.Empty;


#pragma warning restore IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
}
