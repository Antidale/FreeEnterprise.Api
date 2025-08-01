using Dapper;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class RaceRepository(IConnectionProvider connectionProvider, ILogger<RaceRepository> logger) : IRaceRespository
{
    private readonly IConnectionProvider _connectionPrivoder = connectionProvider;

    public async Task<Response<int>> CreateRaceAsync(CreateRaceRoom createRequest)
    {
        using var connection = _connectionPrivoder.GetConnection();

        try
        {
            var model = new Race(createRequest);

            var insertStatement =
    @$"
with try_insert as (
    insert into races.race_detail (
        {nameof(Race.room_name)},
        {nameof(Race.race_host)},
        {nameof(Race.race_type)},
        {nameof(Race.metadata)}
    )
    VALUES
    (
        @{nameof(Race.room_name)},
        @{nameof(Race.race_host)},
        @{nameof(Race.race_type)},
        @{nameof(Race.metadata)}
    )
    ON CONFLICT(room_name) DO NOTHING
    RETURNING id
)
select * from try_insert
UNION
select id from races.race_detail where {nameof(Race.room_name)} = @{nameof(Race.room_name)}
;
";
            var insertResult = await connection.QuerySingleAsync<int>(insertStatement, model);

            return insertResult <= 0
                ? new Response<int>().InternalServerError("patch page was not saved")
                : Response.SetSuccess(insertResult);

        }
        catch (Exception ex)
        {
            logger.LogError("Exception when saving patch page: {ex}", ex.ToString());
            return new Response<int>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response> JoinRaceAsync(JoinRaceRequest request)
    {
        return new Response().InternalServerError("Not Implemented");
        using var connection = _connectionPrivoder.GetConnection();

        //upsert user to race.racers
        //add entrant to race.entrants


    }

    public async Task<Response<IEnumerable<RaceDetail>>> GetRacesAsync(int offset, int limit, string? description, string? flagset)
    {
        using var connection = _connectionPrivoder.GetConnection();
        var query = @$"select
rd.{nameof(Race.id)} as {nameof(RaceDetail.RaceId)},
{nameof(Race.room_name)} as {nameof(RaceDetail.RoomName)},
{nameof(Race.race_host)} as {nameof(RaceDetail.RaceHost)},
{nameof(Race.race_type)} as {nameof(RaceDetail.RaceType)},
{nameof(Race.metadata)} as {nameof(RaceDetail.Metadata)},
{nameof(RolledSeed.flagset)} as {nameof(RaceDetail.Flagset)},
max(rs.{nameof(RolledSeed.id)}) as {nameof(RaceDetail.SeedId)}
FROM races.race_detail rd
left join seeds.rolled_seeds rs on rs.race_id = rd.id
where (@description is null or metadata ->> 'Description' like @description)
and (@flagset is null or rs.flagset_search @@ websearch_to_tsquery('english', @flagset))
group by {nameof(RaceDetail.RoomName)}, {nameof(RaceDetail.RaceHost)}, {nameof(RaceDetail.RaceType)}, {nameof(RaceDetail.Metadata)}, {nameof(RaceDetail.Flagset)}, {nameof(RaceDetail.RaceId)}
order by {nameof(RaceDetail.SeedId)}
offset @offset
limit @limit
;";

        try
        {
            connection.Open();
            var races = await connection.QueryAsync<RaceDetail>(query, new { offset, limit, description = $"%{description}%", flagset });
            races = races.Select(x => x.WithFilteredMetadata("CR_"));

            return Response.SetSuccess(races);
        }
        catch (Exception ex)
        {
            logger.LogError("When fetching races information {ex}", ex.ToString());
            return new Response<IEnumerable<RaceDetail>>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response<RaceDetail>> GetRaceAsync(string idOrSlug)
    {
        using var connection = _connectionPrivoder.GetConnection();
        try
        {
            connection.Open();
            RaceDetail? raceDetail;
            if (int.TryParse(idOrSlug, out var id))
            {
                raceDetail = await connection.QueryFirstOrDefaultAsync<RaceDetail>(GetRaceByIdQueryString(), new { id });
            }
            else
            {
                raceDetail = await connection.QueryFirstOrDefaultAsync<RaceDetail>(GetRaceByRoomNameQuery(), new { roomName = idOrSlug });
            }

            if (raceDetail is null) { return new Response<RaceDetail>().NotFound(idOrSlug); }

            return new Response<RaceDetail>().SetSuccess(raceDetail.WithFilteredMetadata("CR_"));

        }
        catch (Exception ex)
        {
            logger.LogError("When fetching races information for race {idOrSlug}: {ex}", idOrSlug, ex.ToString());
            return new Response<RaceDetail>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response<string>> GetRaceSeedHtmlAsync(string idOrSlug)
    {
        using var connection = _connectionPrivoder.GetConnection();
        try
        {
            connection.Open();
            string? patchHtml;
            if (int.TryParse(idOrSlug, out var id))
            {
                patchHtml = await connection.QueryFirstOrDefaultAsync<string>(GetSeedByRaceIdQuery(), new { id });
            }
            else
            {
                patchHtml = await connection.QueryFirstOrDefaultAsync<string>(GetSeedByRaceRoomNameQuery(), new { roomName = idOrSlug });
            }

            if (patchHtml is null) { return new Response<string>().NotFound(idOrSlug); }

            logger.LogInformation("got the stuff");

            return new Response<string>().SetSuccess(patchHtml);

        }
        catch (Exception ex)
        {
            logger.LogError("When fetching races information for race {idOrSlug}: {ex}", idOrSlug, ex.ToString());
            return new Response<string>().InternalServerError(ex.Message);
        }
    }

    private string GetRaceByIdQueryString() => @$"select
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

    private string GetRaceByRoomNameQuery() => @$"select
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

    private string GetSeedByRaceIdQuery() => @"
with seed_id AS (
    select max (rs.id) as seed_id
    from races.race_detail rd
    left join seeds.rolled_seeds rs on rd.id = rs.race_id
    where rd.id = @id
)

select sh.patch_html
from seeds.saved_html sh
join seed_id rs on sh.rolled_seed_id = rs.seed_id;";

    private string GetSeedByRaceRoomNameQuery() => @"
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
