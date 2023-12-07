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
$@"select {nameof(Tournament.id)}, {nameof(Tournament.tracking_channel_id)}, {nameof(Tournament.tracking_message_id)}, {nameof(Tournament.role_id)}
from tournament.tournaments 
where {nameof(Tournament.guild_id)} = @GuildId 
    and {nameof(Tournament.registration_start)} < now()
    and ({nameof(Tournament.registration_end)} is null or {nameof(Tournament.registration_end)} > now())
    and ({nameof(Tournament.tournament_name)} = @TournamentName or @TournamentName = '');";

        public const string GetEntrantByUserId =
$@"select
      {nameof(Entrant.id)}
    , {nameof(Entrant.user_id)}
    , {nameof(Entrant.user_name)}
    , {nameof(Entrant.pronouns)}
from tournament.entrants
where user_id = @userId";

        public const string InsertEntrantSql =
$@"INSERT INTO tournament.entrants(
    {nameof(Entrant.user_id)},
    {nameof(Entrant.user_name)}, 
    {nameof(Entrant.pronouns)})
VALUES
(@UserId, @UserName, @Pronouns)
Returning id;
";

        public const string GetTournamentSummarySql =
@"WITH registration_counts AS (
    select tournament_id, count(*) as entrant_count
    from tournament.tournament_registrations
    where entrant_id is not null
    group by tournament_id
)

select 
      t.id as TournamentId
    , t.guild_name as GuildName
    , t.tournament_name as TournamentName
    , t.registration_start as RegistrationStart
    , t.registration_end as RegistrationEnd
    , COALESCE(entrant_count, 0) as EntrantCount
from tournament.tournaments t
left join registration_counts r on t.id = r.tournament_id;";

        public const string DropPlayerSql =
@"delete from tournament.registrations
where tournament_id = @tournament_id
and entrant_id = @entrant_id;";
    }
}
