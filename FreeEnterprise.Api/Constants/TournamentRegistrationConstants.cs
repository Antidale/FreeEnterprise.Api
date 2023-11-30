using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Constants
{
    public class TournamentRegistrationConstants
    {
        public const string OpenRegistrationWindowSql = 
$@"update tournament.tournaments
set registration_start = now()
where {nameof(Tournament.id)} = @id;";

        public const string CloseRegistrationWindowSql =
$@"update tournament.tournaments
set registration_end = now()
where {nameof(Tournament.id)} = @id;";

        /// <summary>
        /// Provides sql to search for a tournament that can have registration opened. Registration cannot be opened if the registration period has already been closed for that tournament
        /// </summary>
        public const string SearchRegistrationWindowForOpeningSql =
$@" select {nameof(Tournament.id)}, {nameof(Tournament.tracking_channel_id)}, {nameof(Tournament.tracking_message_id)}
from tournament.tournaments 
where {nameof(Tournament.guild_id)} = @GuildId 
    and ({nameof(Tournament.registration_start)} is null or {nameof(Tournament.registration_start)} > now())
    and ({nameof(Tournament.registration_end)} is null or {nameof(Tournament.registration_end)} > now())
    and ({nameof(Tournament.tournament_name)} = @TournamentName or @TournamentName = '');";

        public const string SearchRegistrationWindowForClosingSql = 
$@"select {nameof(Tournament.id)}, {nameof(Tournament.tracking_channel_id)}, {nameof(Tournament.tracking_message_id)}
from tournament.tournaments 
where {nameof(Tournament.guild_id)} = @GuildId 
    and ({nameof(Tournament.registration_end)} is null or {nameof(Tournament.registration_end)} > now())
    and ({nameof(Tournament.tournament_name)} = @TournamentName or @TournamentName = '');";

        public const string SearchTournamentsForRegistration =
$@"select {nameof(Tournament.id)}, {nameof(Tournament.tracking_channel_id)}, {nameof(Tournament.tracking_message_id)}
from tournament.tournaments 
where {nameof(Tournament.guild_id)} = @GuildId 
    and {nameof(Tournament.registration_start)} < now()
    and ({nameof(Tournament.registration_end)} is null or {nameof(Tournament.registration_end)} > now())
    and ({nameof(Tournament.tournament_name)} = @TournamentName or @TournamentName = '');";

    }
}
