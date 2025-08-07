
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories.Queries;

public class RaceQueries
{
    public const string GetRaceByIdQueryString = @$"select
rd.{nameof(Race.id)} as {nameof(RaceDetail.RaceId)},
{nameof(Race.room_name)} as {nameof(RaceDetail.RoomName)},
{nameof(Race.race_host)} as {nameof(RaceDetail.RaceHost)},
{nameof(Race.race_type)} as {nameof(RaceDetail.RaceType)},
{nameof(Race.metadata)} as {nameof(RaceDetail.Metadata)},
{nameof(RolledSeed.flagset)} as {nameof(RaceDetail.Flagset)},
max(rs.{nameof(RolledSeed.id)}) as {nameof(RaceDetail.SeedId)}
FROM races.race_detail rd
left join seeds.rolled_seeds rs on rs.race_id = rd.id
where rd.id = @id
group by {nameof(RaceDetail.RoomName)}, {nameof(RaceDetail.RaceHost)}, {nameof(RaceDetail.RaceType)}, {nameof(RaceDetail.Metadata)}, {nameof(RaceDetail.Flagset)}, {nameof(RaceDetail.RaceId)}";

    public const string GetRaceByRoomNameQuery = @$"select
rd.{nameof(Race.id)} as {nameof(RaceDetail.RaceId)},
{nameof(Race.room_name)} as {nameof(RaceDetail.RoomName)},
{nameof(Race.race_host)} as {nameof(RaceDetail.RaceHost)},
{nameof(Race.race_type)} as {nameof(RaceDetail.RaceType)},
{nameof(Race.metadata)} as {nameof(RaceDetail.Metadata)},
{nameof(RolledSeed.flagset)} as {nameof(RaceDetail.Flagset)},
max(rs.{nameof(RolledSeed.id)}) as {nameof(RaceDetail.SeedId)}
FROM races.race_detail rd
left join seeds.rolled_seeds rs on rs.race_id = rd.id
where rd.room_name = @roomName
group by {nameof(RaceDetail.RoomName)}, {nameof(RaceDetail.RaceHost)}, {nameof(RaceDetail.RaceType)}, {nameof(RaceDetail.Metadata)}, {nameof(RaceDetail.Flagset)}, {nameof(RaceDetail.RaceId)}";

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
}
