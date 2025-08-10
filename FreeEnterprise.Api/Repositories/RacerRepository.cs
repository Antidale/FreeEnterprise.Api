using Dapper;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Repositories.Queries;

namespace FreeEnterprise.Api.Repositories;

public class RacerRepository(IConnectionProvider connectionProvider, ILogger<RacerRepository> logger) : IRacerRepository
{
    private readonly IConnectionProvider _connectionPrivoder = connectionProvider;

    public async Task<Response<Racer>> GetRacerAsync(string idOrName)
    {
        using var connection = _connectionPrivoder.GetConnection();
        try
        {
            connection.Open();
            _ = int.TryParse(idOrName, out var id);
            var racer = await connection.QuerySingleOrDefaultAsync<Racer>(
                    RacerQueries.GetRacerByIdOrName,
                    new { id, name = idOrName }
                );

            return racer is not null
                ? Response.SetSuccess(racer)
                : Response.NotFound<Racer>($"no racer found by {idOrName}");

        }
        catch (InvalidOperationException ex)
        {
            logger.LogError("Invalid Operation: {ex}", ex);
            if (ex.ToString().Contains("Sequence contains more than one element"))
            {
                return Response.Conflict<Racer>($"Multiple Racers can be identified by {idOrName}, please use a different identification option");
            }
            else return Response.InternalServerError<Racer>(ex.Message.Split(Environment.NewLine).First());
        }
        catch (Exception ex)
        {
            logger.LogError("Error when fetching Racer {idOrName}: {ex}", idOrName, ex.ToString());
            return new Response<Racer>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Racer>>> GetRacersAsync(int offset, int limit, string? name)
    {
        using var connection = _connectionPrivoder.GetConnection();
        try
        {
            connection.Open();
            var racers = await connection.QueryAsync<Racer>(
                RacerQueries.GetRacers,
                new { offset, limit, name });

            return Response.SetSuccess(racers);
        }
        catch (Exception ex)
        {
            logger.LogError("Error when fetching Racers {idOrName}: {ex}", name, ex.ToString());
            return Response.InternalServerError<IEnumerable<Racer>>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<RaceDetail>>> GetRacesForRacerAsync(string idOrName, int offset, int limit)
    {
        var connection = _connectionPrivoder.GetConnection();
        try
        {
            _ = int.TryParse(idOrName, out var entrantId);
            var param = new { entrantId, name = idOrName, offset, limit };

            var races = await connection.QueryAsync<RaceDetail, RaceEntrant, RaceDetail>(
                RacerQueries.GetRacesForRacer,
                (race, entrant) =>
                {
                    race.Entrants.Add(entrant);
                    return race;
                },
                param: param,
                splitOn: nameof(RaceEntrant.RacetimeId).ToLower());

            races = races.GroupBy(x => x.RaceId)
                         .Select(r =>
                            {
                                var race = r.First().WithFilteredMetadata("CR_");
                                race.Entrants = [.. r.Select(x => x.Entrants.Single())];
                                return race;
                            }
                          );


            return Response.SetSuccess(races);
        }
        catch (Exception ex)
        {
            logger.LogError("Error when fetching Racers {idOrName}: {ex}", idOrName, ex.ToString());
            return Response.InternalServerError<IEnumerable<RaceDetail>>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<RaceDetail>>> GetHeadToHeadAsync(string idOrName, string opponentIdOrName)
    {
        using var connection = _connectionPrivoder.GetConnection();

        try
        {
            _ = int.TryParse(idOrName, out var racerId);
            _ = int.TryParse(opponentIdOrName, out var opponentId);
            var param = new
            {
                racerId,
                opponentId,
                racerName = idOrName,
                opponentName = opponentIdOrName
            };

            var results = await connection.QueryAsync<RaceDetail, RaceEntrant, RaceDetail>(
                RacerQueries.GetHeadToHeadForRacers,
                (race, entrant) =>
                {
                    race.Entrants.Add(entrant);
                    return race;
                },
                param: param,
                splitOn: nameof(RaceEntrant.RacetimeId).ToLower()
            );

            var finalzed = results.GroupBy(x => x.RaceId).Select(r =>
            {
                var race = r.First();
                race.Entrants = [.. r.Select(x => x.Entrants.Single())];
                return race;
            });

            return Response.SetSuccess(finalzed);


        }
        catch (Exception ex)
        {
            logger.LogError("Error when fetching H2H for Racers {idOrName} and {opponentIdOrName}: {ex}", idOrName, opponentIdOrName, ex.ToString());
            return Response.InternalServerError<IEnumerable<RaceDetail>>(ex.Message);
        }
    }

}
