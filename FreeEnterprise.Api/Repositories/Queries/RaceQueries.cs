
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories.Queries;

public class RaceQueries
{
    public const string GetRaceByIdQueryString = @$"{GetRaceBySelect}
where rd.id = @id
{GetRaceByGroupBy}";

    public const string GetRaceByRoomNameQuery = @$"{GetRaceBySelect}
where rd.room_name = @roomName
{GetRaceByGroupBy}";

    public const string GetSeedByRaceIdQuery = @"
with seed_id AS (
    select max (rs.id) as seed_id
    from races.race_detail rd
    left join seeds.rolled_seeds rs on rd.id = rs.race_id
    where rd.id = @id
)

select sh.patch_html
from seeds.saved_html sh
join seed_id rs on sh.rolled_seed_id = rs.seed_id;";

    public const string GetSeedByRaceRoomNameQuery = @"
with seed_id AS (
    select max (rs.id) as seed_id
    from races.race_detail rd
    left join seeds.rolled_seeds rs on rd.id = rs.race_id
    where rd.room_name = @roomName
)

select sh.patch_html
from seeds.saved_html sh
join seed_id rs on sh.rolled_seed_id = rs.seed_id;";

    public const string GetEntrantsForRace = """
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
    , '{"comment": "", "score": "", "scoreChange": ""}' || re.metadata as EntrantMetadata
    , re.placement
    , r.racetime_display_name as Name
from races.race_entrants re
join races.race_detail rd on re.race_id = rd.id
join races.racers r on re.entrant_id = r.id
left join seeds.rolled_seeds rs on rs.race_id = r.id
where rd.id = @id
group by RaceId, RoomName, RaceHost, RaceType, EndedAt, Flagset, rd.metadata, placement, Name, RacetimeId, TwitchName, FinishTime, EntrantMetadata
order by placement;
""";

    private const string GetRaceBySelect = @$"select
rd.{nameof(Race.id)} as {nameof(RaceDetail.RaceId)},
{nameof(Race.room_name)} as {nameof(RaceDetail.RoomName)},
{nameof(Race.race_host)} as {nameof(RaceDetail.RaceHost)},
{nameof(Race.race_type)} as {nameof(RaceDetail.RaceType)},
{nameof(Race.metadata)} as {nameof(RaceDetail.Metadata)},
{nameof(Race.ended_at)} as {nameof(RaceDetail.EndedAt)},
{nameof(RolledSeed.flagset)} as {nameof(RaceDetail.Flagset)},
max(rs.{nameof(RolledSeed.id)}) as {nameof(RaceDetail.SeedId)}
FROM races.race_detail rd
left join seeds.rolled_seeds rs on rs.race_id = rd.id";

    private const string GetRaceByGroupBy = @$"group by {nameof(RaceDetail.RoomName)}, {nameof(RaceDetail.RaceHost)}, {nameof(RaceDetail.RaceType)}, {nameof(RaceDetail.Metadata)}, {nameof(RaceDetail.Flagset)}, {nameof(RaceDetail.RaceId)}, {nameof(RaceDetail.EndedAt)}";


}
