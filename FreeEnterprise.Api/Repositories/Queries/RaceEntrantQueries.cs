using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories.Queries;

public class RaceEntrantQueries
{
    public const string GetEntrantsByRaceId = @$"select 
      {nameof(Racer.racetime_display_name)} as {nameof(RaceEntrant.Name)}
    , {nameof(Racer.twitch_name)} as {nameof(RaceEntrant.TwitchName)}
    , {nameof(RaceEntrantModel.finish_time)} as {nameof(RaceEntrant.FinishTime)}
    , {nameof(RaceEntrantModel.placement)}
    , {nameof(RaceEntrantModel.metadata)}
from races.race_entrants re
join races.racers r on re.entrant_id = r.id
where re.race_id = @raceId;";

    public const string GetEntrantsByRaceName = @$"select
      {nameof(Racer.racetime_display_name)} as {nameof(RaceEntrant.Name)}
    , {nameof(Racer.twitch_name)} as {nameof(RaceEntrant.TwitchName)}
    , {nameof(RaceEntrantModel.finish_time)} as {nameof(RaceEntrant.FinishTime)}
    , {nameof(RaceEntrantModel.placement)}
    , {nameof(RaceEntrantModel.metadata)}
from races.race_entrants re
join races.racers r on re.entrant_id = r.id
join races.race_detail rd on re.race_id = rd.id
where rd.room_name = @roomName;";
}
