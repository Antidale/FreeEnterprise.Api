using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Repositories.Queries;

public class RaceEntrantQueries
{
    public const string GetRacerById = @$"";

    public const string GetRacerByName = @$"";
    public const string GetEntrantsByRaceId = $$"""
select 
      {{nameof(Models.Racer.racetime_display_name)}} as {{nameof(RaceEntrant.Name)}}
    , {{nameof(Models.Racer.twitch_name)}} as {{nameof(RaceEntrant.TwitchName)}}
    , {{nameof(Models.RaceEntrantModel.finish_time)}} as {{nameof(RaceEntrant.FinishTime)}}
    , {{nameof(Models.Racer.racetime_id)}} as {{nameof(RaceEntrant.RacetimeId)}}
    , {{nameof(Models.RaceEntrantModel.placement)}}
    , '{"comment": "", "score": "", "scoreChange": ""}'::jsonb || re.{{nameof(Models.RaceEntrantModel.metadata)}} as {{nameof(RaceEntrant.EntrantMetadata)}}
from races.race_entrants re
join races.racers r on re.entrant_id = r.id
where re.race_id = @raceId
order by placement;
""";

    public const string GetEntrantsByRaceName = $$"""
select 
      {{nameof(Models.Racer.racetime_display_name)}} as {{nameof(RaceEntrant.Name)}}
    , {{nameof(Models.Racer.twitch_name)}} as {{nameof(RaceEntrant.TwitchName)}}
    , {{nameof(Models.RaceEntrantModel.finish_time)}} as {{nameof(RaceEntrant.FinishTime)}}
    , {{nameof(Models.Racer.racetime_id)}} as {{nameof(RaceEntrant.RacetimeId)}}
    , {{nameof(Models.RaceEntrantModel.placement)}}
    , '{"comment": "", "score": "", "scoreChange": ""}'::jsonb || re.{{nameof(Models.RaceEntrantModel.metadata)}} as {{nameof(RaceEntrant.EntrantMetadata)}}
from races.race_entrants re
join races.racers r on re.entrant_id = r.id
join races.race_detail rd on re.race_id = rd.id
where rd.room_name = @roomName
order by placement;
""";
}
