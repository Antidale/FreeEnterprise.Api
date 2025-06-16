using Dapper;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class SeedRepository(IConnectionProvider connectionProvider, ILogger<SeedRepository> logger) : ISeedRepository
{
    private readonly IConnectionProvider _connectionProvider = connectionProvider;

    public async Task<Response<int>> SaveSeedRolledAsync(LogSeedRoled seedInfo)
    {
        using var connection = _connectionProvider.GetConnection();
        try
        {
            connection.Open();
            var raceId = seedInfo.RaceId;

            //In case a caller doesn't know our internal id (e.g. racingway picks a race that another bot created for rt.gg), we can try to fetch the id by the room name. If neither works, we just dont' worry about it.
            if (seedInfo.RaceId is null && seedInfo.RaceName is not null)
            {
                var lookupSql = @$"select id from races.race_detail where {nameof(Race.room_name)} = @RoomName";
                raceId = await connection.QueryFirstOrDefaultAsync<int?>(lookupSql, new { RoomName = seedInfo.RaceName });
                logger.LogCritical("pulled {raceId} from {raceName}", raceId, seedInfo.RaceName);
            }

            var sql =
@$"insert into seeds.rolled_seeds(
    {nameof(RolledSeed.user_id)},
    {nameof(RolledSeed.flagset)},
    {nameof(RolledSeed.link)},
    {nameof(RolledSeed.fe_version)},
    {nameof(RolledSeed.seed)},
    {nameof(RolledSeed.verification)},
    {nameof(RolledSeed.race_id)}
)
Values(@UserId, @Flags, @Url, @Version, @Seed, @Verification, @RaceId)
RETURNING id;";

            var insertResponse = await connection.QuerySingleAsync<int>(sql, new
            {
                UserId = seedInfo.UserId.ToString(),
                seedInfo.Info.Flags,
                seedInfo.Info.Url,
                seedInfo.Info.Version,
                seedInfo.Info.Seed,
                seedInfo.Info.Verification,
                raceId
            });

            return new Response<int>().SetSuccess(insertResponse);
        }
        catch (Exception ex)
        {
            return new Response<int>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response> SavePatchHtml(int savedSeedId, string html)
    {
        using var connection = _connectionProvider.GetConnection();

        var insertStatment =
@$"insert into seeds.saved_html (rolled_seed_id, patch_html)
    VALUES (@savedSeedId, @html);
";
        try
        {
            var insertResponse = await connection.ExecuteAsync(insertStatment, new { savedSeedId, html });
            return insertResponse <= 0
                ? new Response().InternalServerError("patch page was not saved")
                : Response.SetSuccess();
        }
        catch (Exception ex)
        {
            logger.LogError("Exception when saving patch page: {ex}", ex.ToString());
            return new Response().InternalServerError(ex.Message);
        }
    }
}
