namespace FreeEnterprise.Api.Models
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database

    /// <summary>
    /// Model correspinding to the data structure of the tournament.tournament_registrations table
    /// </summary>
    public class TournamentRegistration
    {
        public required string guild_id { get; init; }
        public required string guild_name { get; init; }
        public int tournament_id { get; init; }
        public required string tournament_name { get; init; }
        public DateTimeOffset? registration_start { get; init; }
        public DateTimeOffset? registration_end { get; init; }
        public int? entrant_id { get; init; }
        public string user_id { get; init; } = string.Empty;
        public string user_name { get; init; } = string.Empty;
        public string pronouns { get; init; } = string.Empty;
        public DateTime? registered_on { get; init; }

    }

#pragma warning restore IDE1006 // Naming Styles
}
