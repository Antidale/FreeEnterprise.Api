namespace FreeEnterprise.Api.Models
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database

    /// <summary>
    /// Database model for a tournament
    /// </summary>
    public class Tournament
    {
        public int id { get; set; }
        public string guild_id { get; set; } = string.Empty;
        public string guild_name { get; set; } = string.Empty;
        public string tracking_channel_id { get; set; } = string.Empty;
        public string tracking_message_id { get; set; } = string.Empty;
        public string tournament_name { get; set; } = string.Empty;
        public string rules_link { get; set; } = string.Empty;
        public string standings_link { get; set; } = string.Empty;
        public string role_id { get; set; } = string.Empty;
        public DateTimeOffset? registration_start { get; set; }
        public DateTimeOffset? registration_end { get; set; }

    }

#pragma warning restore IDE1006 // Naming Styles
}
