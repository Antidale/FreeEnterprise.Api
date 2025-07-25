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
        using var connection = _connectionPrivoder.GetConnection();

        //upsert user to race.racers
        //add entrant to race.entrants

        return new Response().BadRequest("stuff");
    }

    public async Task<Response<IEnumerable<RaceDetail>>> GetRacesAsync()
    {
        using var connection = _connectionPrivoder.GetConnection();
        var query = @$"select 
{nameof(Race.room_name)} as {nameof(RaceDetail.RoomName)},
{nameof(Race.race_host)} as {nameof(RaceDetail.RaceHost)},
{nameof(Race.race_type)} as {nameof(RaceDetail.RaceType)},
{nameof(Race.metadata)} as {nameof(RaceDetail.Metadata)}
FROM races.race_detail;";
        try
        {
            connection.Open();
            var races = await connection.QueryAsync<RaceDetail>(query);

            return Response.SetSuccess(races);
        }
        catch (Exception ex)
        {
            logger.LogError("Exception when saving patch page: {ex}", ex.ToString());
            return new Response<IEnumerable<RaceDetail>>().InternalServerError(ex.Message);
        }
    }
}
