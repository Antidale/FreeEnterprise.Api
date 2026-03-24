using Dapper;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class RaceEntrantRepository(IConnectionProvider connectionProvider, ILogger<RaceEntrantRepository> logger) : IRaceEntrantRepository
{
    public async Task InsertRaceEntrants(List<CreateRaceEntrantModel> entrants)
    {
        using var connection = connectionProvider.GetConnection();
        try
        {
            var query = """
insert into races.race_entrants(race_id, entrant_id, finish_time, placement, metadata)
select  rd.id, ra.id, ins.finish_time, ins.placement, ins.metadata
from(
    values(@finish_time, @placement, @metadata, @room_name, @racetime_id)
) as ins(finish_time, placement, metadata, room_name, racetime_id)
join races.race_detail rd on rd.room_name = ins.room_name
join races.racers ra on ra.racetime_display_name = ins.racetime_id
""";
            await connection.ExecuteAsync(query, entrants);
        }
        catch (Exception ex)
        {
            logger.LogError("Error inserting race entrants {ex}", ex.ToString());
        }

    }
}
