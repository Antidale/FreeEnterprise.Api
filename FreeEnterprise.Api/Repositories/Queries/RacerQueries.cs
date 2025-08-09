namespace FreeEnterprise.Api.Repositories.Queries;

public class RacerQueries
{
  public const string GetRacerById = @$"select 
      racetime_display_name as RacetimeName
    , twitch_name as TwitchName
    , racetime_id as RacetimeId
from races.racers
where id = @id
;";

  public const string GetRacerByName = @$"select 
      racetime_display_name as RacetimeName
    , twitch_name as TwitchName
    , racetime_id as RacetimeId
from races.racers
where racetime_display_name = @name
or twitch_name = @name
or racetime_id = @name
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

  public const string GetRacesForRacer = """
select 
      rd.room_name as RoomName
    , rd.id as RaceId
    , rd.race_type as RaceType
    , rd.race_host as RaceHost
    , rs.flagset as Flagset
    , rd.ended_at as EndedAt
    , max(rs.id) as SeedId
    , rd.metadata 
    
    , r.racetime_id as RacetimeId
    , r.twitch_name as TwitchName
    , re.finish_time as FinishTime
    , '{"comment": "", "score": "", "scoreChange": ""}'::jsonb || re.metadata as EntrantMetadata
    , re.placement
    , r.racetime_display_name as Name
from races.race_entrants re
join races.race_detail rd on re.race_id = rd.id
join races.racers r on re.entrant_id = r.id
left join seeds.rolled_seeds rs on rs.race_id = r.id
where re.entrant_id = @entrantId or r.racetime_id = @racetimeId
group by RaceId, RoomName, RaceHost, RaceType, EndedAt, Flagset, rd.metadata, placement, Name, RacetimeId, TwitchName, FinishTime, EntrantMetadata
order by rd.ended_at
offset @offset
limit @limit
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
