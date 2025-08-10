using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories.Queries;

public class RacerQueries
{
  public const string GetRacerByIdOrName = @$"select
      racetime_display_name as RacetimeName
    , twitch_name as TwitchName
    , racetime_id as RacetimeId
from races.racers
where id = @id
or lower(racetime_display_name) = lower(@name)
or lower(twitch_name) = lower(@name)
or lower(racetime_id) = lower(@name)
;";

  public const string GetRacers = @$"select
    racetime_display_name as RacetimeName
    , twitch_name as TwitchName
    , racetime_id as RacetimeId
from races.racers
Where @name is null 
or racetime_display_name = @name
or twitch_name = @name
order by RacetimeName
offset @offset
limit @limit
;";

  public const string GetRacesForRacer = $$"""
with race_data as (
  select
    rd.{{nameof(Race.id)}} as {{nameof(RaceDetail.RaceId)}},
    {{nameof(Race.room_name)}} as {{nameof(RaceDetail.RoomName)}},
    {{nameof(Race.race_host)}} as {{nameof(RaceDetail.RaceHost)}},
    {{nameof(Race.race_type)}} as {{nameof(RaceDetail.RaceType)}},
    rd.{{nameof(Race.metadata)}} as {{nameof(RaceDetail.Metadata)}},
    {{nameof(Race.ended_at)}} as {{nameof(RaceDetail.EndedAt)}},
    {{nameof(RolledSeed.flagset)}} as {{nameof(RaceDetail.Flagset)}},
    max(rs.{{nameof(RolledSeed.id)}}) as {{nameof(RaceDetail.SeedId)}}
  FROM races.race_detail rd
    join races.race_entrants re on rd.id = re.race_id
    join races.racers r on re.entrant_id = r.id
    left join seeds.rolled_seeds rs on rs.race_id = rd.id
  where r.id = @entrantId
    or lower(racetime_display_name) = lower(@name)
    or lower(twitch_name) = lower(@name)
    or lower(racetime_id) = lower(@name)
  group by {{nameof(RaceDetail.RoomName)}}, {{nameof(RaceDetail.RaceHost)}}, {{nameof(RaceDetail.RaceType)}}, rd.{{nameof(RaceDetail.Metadata)}}, {{nameof(RaceDetail.Flagset)}}, {{nameof(RaceDetail.RaceId)}}, {{nameof(RaceDetail.EndedAt)}}
  offset @offset
  limit @limit
)

select rd.*
    , r.racetime_id as RacetimeId
    , r.twitch_name as TwitchName
    , re.finish_time as FinishTime
    , '{"comment": "", "score": "", "scoreChange": ""}' || re.metadata as EntrantMetadata
    , re.placement
    , r.racetime_display_name as Name
from race_data rd
join races.race_entrants re on rd.raceId = re.race_id
join races.racers r on r.id = re.entrant_id
order by endedat desc, placement

;
""";

  public const string GetHeadToHeadForRacers = """
with 
racer_one as (
    select r.racetime_id, r.twitch_name, r.racetime_display_name, re.finish_time, re.placement, '{"comment": "", "score": "", "scoreChange": ""}'::jsonb || re.metadata as EntrantMetadata, re.race_id
    from races.racers r 
    join races.race_entrants re on r.id = re.entrant_id
    where lower(r.racetime_id) = lower(@racerName)
    or lower(r.twitch_name) = lower(@racerName)
    or lower(r.racetime_display_name) = lower(@racerName)
    or r.id = @racerId
),

racer_two as (
    select r.racetime_id, r.twitch_name, r.racetime_display_name, re.finish_time, re.placement, '{"comment": "", "score": "", "scoreChange": ""}'::jsonb || re.metadata as EntrantMetadata, re.race_id
    from races.racers r 
    join races.race_entrants re on r.id = re.entrant_id
    where lower(r.racetime_id) = lower(@opponentName)
    or lower(r.twitch_name) = lower(@opponentName)
    or lower(r.racetime_display_name) = lower(@opponentName)
    or r.id = @opponentId
)

select 
      rd.id as RaceId
    , rd.room_name as RoomName
    , rd.race_type as RaceType
    , rd.race_host as RaceHost
    , rd.ended_at as EndedAt
    , rd.metadata
    , r.racetime_id as RacetimeId
    , r.twitch_name as TwitchName
    , r.racetime_display_name as Name
    , r.finish_time as FinishTime
    , r.placement
    , r.EntrantMetadata
from races.race_detail rd
join racer_one r on rd.id = r.race_id
join racer_two on rd.id = racer_two.race_id
UNION
select 
      rd.id as RaceId
    , rd.room_name as RoomName
    , rd.race_type as RaceType
    , rd.race_host as RaceHost
    , rd.ended_at as EndedAt
    , rd.metadata
    , r.racetime_id as RacetimeId
    , r.twitch_name as TwitchName
    , r.racetime_display_name as Name
    , r.finish_time as FinishTime
    , r.placement
    , r.EntrantMetadata
from races.race_detail rd
join racer_one on rd.id = racer_one.race_id
join racer_two r on rd.id = r.race_id
order by EndedAt, placement
;
""";

}
