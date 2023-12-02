namespace FreeEnterprise.Api.Models
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
    public class Registration
    {
        public int tournament_id { get; init; } 
        public int entrant_id { get; init; }
        public string user_name_anlias { get; init; } = string.Empty;
        public DateTimeOffset registered_on { get; init; }

    }
#pragma warning restore IDE1006 // Naming Styles
}
