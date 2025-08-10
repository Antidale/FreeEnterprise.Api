using Dapper;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Repositories.Queries;

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

    // public async Task<Response> JoinRaceAsync(JoinRaceRequest request)
    // {
    //     return new Response().InternalServerError("Not Implemented");


    //     //upsert user to race.racers
    //     //add entrant to race.entrants
    // }

    public async Task<Response<IEnumerable<RaceDetail>>> GetRacesAsync(int offset, int limit, bool includeCancelled, string? description, string? flagset)
    {
        using var connection = _connectionPrivoder.GetConnection();

        try
        {
            connection.Open();
            var races = await connection.QueryAsync<RaceDetail, RaceEntrant, RaceDetail>(
                RaceQueries.GetRacesQuery,
                (race, entrant) =>
                {
                    race.Entrants.Add(entrant);
                    return race;
                },
                param: new { offset, limit, description = $"%{description}%", flagset, includeCancelled },
                splitOn: nameof(RaceEntrant.RacetimeId).ToLower()
                );

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

            _ = int.TryParse(idOrSlug, out var id);

            var raceDetail = await connection.QueryAsync<RaceDetail, RaceEntrant, RaceDetail>(
                    sql: RaceQueries.GetRaceByIdQueryString,
                    map: (race, entrant) =>
                    {
                        race.Entrants.Add(entrant);
                        return race;
                    },
                    param: new { id, roomName = idOrSlug },
                    splitOn: nameof(RaceEntrant.RacetimeId).ToLower()
                );

            if (raceDetail is null || !raceDetail.Any()) { return new Response<RaceDetail>().NotFound(idOrSlug); }

            raceDetail = raceDetail.GroupBy(x => x.RaceId)
                         .Select(r =>
                            {
                                var race = r.First().WithFilteredMetadata("CR_");
                                race.Entrants = [.. r.Select(x => x.Entrants.Single())];
                                return race;
                            }
                          );

            return new Response<RaceDetail>().SetSuccess(raceDetail.First());

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
                patchHtml = await connection.QueryFirstOrDefaultAsync<string>(RaceQueries.GetSeedByRaceIdQuery, new { id });
            }
            else
            {
                patchHtml = await connection.QueryFirstOrDefaultAsync<string>(RaceQueries.GetSeedByRaceRoomNameQuery, new { roomName = idOrSlug });
            }

            if (patchHtml is null) { return new Response<string>().NotFound(idOrSlug); }

            return new Response<string>().SetSuccess(patchHtml);

        }
        catch (Exception ex)
        {
            logger.LogError("When fetching races information for race {idOrSlug}: {ex}", idOrSlug, ex.ToString());
            return new Response<string>().InternalServerError(ex.Message);
        }
    }
}
