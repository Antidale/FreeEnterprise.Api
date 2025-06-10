
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
    @$"insert into races.race_detail (
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
RETURNING id;
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
}
