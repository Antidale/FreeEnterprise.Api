namespace FreeEnterprise.Api.DTOs
{
    public class CreateTournamentRequest
    {
        public ulong GuildId {get; set;}
        public required string GuildName {get; set;}
        public ulong ChannelId {get; set;}
        public required string TournamentName { get; set; }
        public DateTimeOffset RegistrationStart { get; set; }
        public DateTimeOffset RegistrationEnd { get; set; }
    }
}
