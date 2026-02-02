using System.Text.RegularExpressions;
using Dapper;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public partial class SeedRepository(IConnectionProvider connectionProvider, ILogger<SeedRepository> logger) : ISeedRepository
{
    private readonly IConnectionProvider _connectionProvider = connectionProvider;

    public async Task<Response<int>> SaveSeedRolledAsync(LogSeedRoled seedInfo)
    {
        using var connection = _connectionProvider.GetConnection();
        try
        {
            connection.Open();
            var raceId = seedInfo.RaceId;

            var match = UrlFlagsRegex().Matches(seedInfo.Info.Url);

            //TODO: move this out to somewhere better
            var binaryFlags = UrlFlagsRegex().Matches(seedInfo.Info.Url).FirstOrDefault()?.Groups.Values.LastOrDefault()?.Value ?? "";

            //In case a caller doesn't know our internal id (e.g. racingway picks a race that another bot created for rt.gg), we can try to fetch the id by the room name. If neither works, we just don't worry about it.
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
    {nameof(RolledSeed.binary_flags)},
    {nameof(RolledSeed.race_id)}
)
Values(@UserId, @Flags, @Url, @Version, @Seed, @Verification, @BinaryFlags, @RaceId)
ON CONFLICT(link) DO NOTHING
RETURNING id;";

            var insertResponse = await connection.QuerySingleAsync<int>(sql, new
            {
                UserId = seedInfo.UserId.ToString(),
                seedInfo.Info.Flags,
                seedInfo.Info.Url,
                seedInfo.Info.Version,
                seedInfo.Info.Seed,
                seedInfo.Info.Verification,
                BinaryFlags = binaryFlags,
                raceId
            });

            return new Response<int>().SetSuccess(insertResponse);
        }
        catch (Exception ex)
        {
            return new Response<int>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<SeedDetail>>> SearchSeedDetails(int offset, int limit, string? flagset, string? binaryFlags, string? seedValue)
    {
        using var connection = _connectionProvider.GetConnection();
        try
        {
            var query = $@"select 
    {nameof(RolledSeed.id)} as {nameof(SeedDetail.SeedId)}
    , {nameof(RolledSeed.flagset)} as {nameof(SeedDetail.Flagset)}
    , {nameof(RolledSeed.verification)} as {nameof(SeedDetail.Verification)}
    , {nameof(RolledSeed.link)} as {nameof(SeedDetail.SourceUrl)}
    , {nameof(RolledSeed.seed)} as {nameof(SeedDetail.Seed)}
    , {nameof(RolledSeed.fe_version)} as {nameof(SeedDetail.Version)}
    , ts_rank({nameof(RolledSeed.flagset_search)}, websearch_to_tsquery('english', @flagset)) as Rank
from seeds.rolled_seeds
where (@seedValue is null or {nameof(RolledSeed.seed)} = @seedValue)
and (@binaryFlags is null or {nameof(RolledSeed.binary_flags)} = @binaryFlags)
and (@flagset is null or {nameof(RolledSeed.flagset_search)} @@ websearch_to_tsquery('english', @flagset))
order by Rank desc, id
offset @offset
limit @limit
;";
            var seeds = await connection.QueryAsync<SeedDetail>(query, new { seedValue, flagset, binaryFlags, offset, limit });

            return new Response<IEnumerable<SeedDetail>>().SetSuccess(seeds);

        }
        catch (Exception ex)
        {
            logger.LogError("Exception when searching: \r\nflagset: {flagset}\r\nbinaryFlags {binaryFlags}\r\nseedValue: {seedValue}\r\n{ex}", flagset, binaryFlags, seedValue, ex.ToString());
            return new Response<IEnumerable<SeedDetail>>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response<SeedDetail>> GetSeedByIdAsync(int id)
    {
        using var connection = _connectionProvider.GetConnection();
        try
        {
            var query = $@"select 
    {nameof(RolledSeed.id)} as {nameof(SeedDetail.SeedId)}
    , {nameof(RolledSeed.flagset)} as {nameof(SeedDetail.Flagset)}
    , {nameof(RolledSeed.verification)} as {nameof(SeedDetail.Verification)}
    , {nameof(RolledSeed.link)} as {nameof(SeedDetail.SourceUrl)}
    , {nameof(RolledSeed.seed)} as {nameof(SeedDetail.Seed)}
    , {nameof(RolledSeed.fe_version)} as {nameof(SeedDetail.Version)}
    , cast(0 as real) as Rank
from seeds.rolled_seeds
where id = @id    
";
            var seedDetail = await connection.QuerySingleOrDefaultAsync<SeedDetail>(query, new { id });
            if (seedDetail is null)
            {
                return new Response<SeedDetail>().NotFound($"Seed id {id} not found");
            }

            return new Response<SeedDetail>().SetSuccess(seedDetail);
        }
        catch (Exception ex)
        {
            logger.LogError("Exception when searching for {id}: \r\n{ex}", id, ex.ToString());
            return new Response<SeedDetail>().InternalServerError(ex.Message);
        }
    }

    public async Task<Response<string>> GetPatchBySeedIdAsync(int id)
    {
        using var connection = _connectionProvider.GetConnection();
        try
        {
            var query = "select patch_html from seeds.saved_html where saved_html.rolled_seed_id = @id";
            var patchHtml = await connection.QuerySingleOrDefaultAsync<string>(query, new { id });
            if (string.IsNullOrEmpty(patchHtml))
            {
                return new Response<string>().NotFound($"No patch page found for seed {id}");
            }

            return new Response<string>().SetSuccess(patchHtml);
        }
        catch (Exception ex)
        {
            logger.LogError("Exception when getting patch for seed {id}: \r\n{ex}", id, ex.ToString());
            return new Response<string>().InternalServerError(ex.Message);
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

    [GeneratedRegex(@"=([\w\-]+)\.", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex UrlFlagsRegex();
}
