
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories.Queries;

public class RaceQueries
{
    public const string GetRaceByIdQueryString = $$"""
    with race_data as (
    {{GetRaceBySelect}}
where rd.id = @id or rd.room_name = @roomName
{{GetRaceByGroupBy}}
    )
{{GetEntrantsForRaceSubsection}}
""";

    public const string GetRacesQuery = $$"""
with race_data as (
    {{GetRaceBySelect}}
    where (@description is null or metadata ->> 'Description' like @description)
    and (@flagset is null or rs.flagset_search @@ websearch_to_tsquery('english', @flagset))
    and (
            @includeCancelled is true
            or metadata ->> 'Status' != 'cancelled'
            or metadata ->> 'Status' is null
        )
    {{GetRaceByGroupBy}}
    order by EndedAt desc
    offset @offset
    limit @limit
)
{{GetEntrantsForRaceSubsection}}
""";

    public const string GetRaceSeedQuery = @"
with seed_id AS (
    select max (rs.id) as seed_id
    from races.race_detail rd
    left join seeds.rolled_seeds rs on rd.id = rs.race_id
    where rd.id = @id 
    or rd.room_name = @roomName
)

select sh.patch_html
from seeds.saved_html sh
join seed_id rs on sh.rolled_seed_id = rs.seed_id;";

    private const string GetRaceBySelect = $$"""
    select
rd.{{nameof(Race.id)}} as {{nameof(RaceDetail.RaceId)}},
{{nameof(Race.room_name)}} as {{nameof(RaceDetail.RoomName)}},
{{nameof(Race.race_host)}} as {{nameof(RaceDetail.RaceHost)}},
{{nameof(Race.race_type)}} as {{nameof(RaceDetail.RaceType)}},
{{nameof(Race.metadata)}} as {{nameof(RaceDetail.Metadata)}},
{{nameof(Race.ended_at)}} as {{nameof(RaceDetail.EndedAt)}},
{{nameof(RolledSeed.flagset)}} as {{nameof(RaceDetail.Flagset)}},
max(rs.{{nameof(RolledSeed.id)}}) as {{nameof(RaceDetail.SeedId)}}
FROM races.race_detail rd
left join seeds.rolled_seeds rs on rs.race_id = rd.id
""";

    private const string GetRaceByGroupBy = @$"group by {nameof(RaceDetail.RoomName)}, {nameof(RaceDetail.RaceHost)}, {nameof(RaceDetail.RaceType)}, {nameof(RaceDetail.Metadata)}, {nameof(RaceDetail.Flagset)}, {nameof(RaceDetail.RaceId)}, {nameof(RaceDetail.EndedAt)}";

    private const string GetEntrantsForRaceSubsection = """

select rd.*
    , r.racetime_id as RacetimeId
    , r.twitch_name as TwitchName
    , re.finish_time as FinishTime
    , '{"comment": "", "score": "", "scoreChange": ""}' || re.metadata as EntrantMetadata
    , re.placement
    , r.racetime_display_name as Name
from race_data rd
left join races.race_entrants re on rd.raceId = re.race_id
left join races.racers r on r.id = re.entrant_id
order by endedat desc, placement
""";


}
