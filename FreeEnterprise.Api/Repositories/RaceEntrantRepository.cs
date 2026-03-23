using System;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Repositories;

public class RaceEntrantRepository(IConnectionProvider connectionProvider, ILogger<RaceEntrantRepository> logger) : IRaceEntrantRepository
{
    public async Task InsertRaceEntrants()
    {
        using var connection = connectionProvider.GetConnection();
        try
        {
            //do the work    
        }
        catch (Exception ex)
        {
            logger.LogError("Error inserting race entrants {ex}", ex.ToString());
        }

    }
}
