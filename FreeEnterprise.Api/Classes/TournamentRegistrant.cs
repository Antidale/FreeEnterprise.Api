using System;

namespace FreeEnterprise.Api.Classes;

public class TournamentRegistrant
{
    public string TournamentName { get; set; } = string.Empty;
    public string DiscordName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public string Pronouns { get; set; } = string.Empty;
    public string TwitchName { get; set; } = string.Empty;
}
