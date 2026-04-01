using Dapper;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class RaceEntrantRepository(IConnectionProvider connectionProvider, ILogger<RaceEntrantRepository> logger) : IRaceEntrantRepository
{
    public async Task<Response> InsertRaceEntrants(List<CreateRaceEntrantModel> entrants)
    {
        if (entrants.Count < 1)
        {
            return Response.SetSuccess();
        }

        using var connection = connectionProvider.GetConnection();
        try
        {
            var query = """
insert into races.race_entrants(race_id, entrant_id, finish_time, placement, metadata)
select  rd.id, ra.id, ins.finish_time, ins.placement, ins.metadata
from(
    values(@finish_time::interval, @placement, @metadata, @room_name, @racetime_id)
) as ins(finish_time, placement, metadata, room_name, racetime_id)
join races.race_detail rd on rd.room_name = ins.room_name
join races.racers ra on ra.racetime_id = ins.racetime_id
""";
            var result = await connection.ExecuteAsync(query, entrants);
            if (result == 0)
            {
                return new Response().InternalServerError("No records inserted");
            }
            return Response.SetSuccess();
        }
        catch (Exception ex)
        {
            logger.LogError("Error inserting race entrants {ex}", ex.ToString());
            return new Response().InternalServerError(ex.Message);
        }

    }
}
