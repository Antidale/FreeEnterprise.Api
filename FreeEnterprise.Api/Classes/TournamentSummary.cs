namespace FreeEnterprise.Api.Classes
{
    public class TournamentSummary
    {
        public int TournamentId { get; set; }
        public string GuildName { get; set; } = string.Empty;
        public string TournamentName { get; set; } = string.Empty;
        public DateTimeOffset RegistrationStart { get; set; }
        public DateTimeOffset RegistrationEnd { get; set; }
        public int EntrantCount { get; set; }

    }
}
