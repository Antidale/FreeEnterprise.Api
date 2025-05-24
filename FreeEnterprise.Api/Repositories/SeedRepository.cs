using Dapper;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class SeedRepository(IConnectionProvider connectionProvider) : ISeedRepository
{
    private readonly IConnectionProvider _connectionProvider = connectionProvider;

    public async Task<Response<int>> SaveSeedRolledAsync(LogSeedRoled seedInfo)
    {
        using var connection = _connectionProvider.GetConnection();

        try
        {
            connection.Open();

            var sql =
@$"insert into seeds.rolled_seeds(
    {nameof(RolledSeed.user_id)},
    {nameof(RolledSeed.flagset)},
    {nameof(RolledSeed.link)},
    {nameof(RolledSeed.fe_version)},
    {nameof(RolledSeed.seed)},
    {nameof(RolledSeed.verification)}
)
Values(@UserId, @Flags, @Url, @Version, @Seed, @Verification);";

            var insertResponse = await connection.ExecuteAsync(sql, new
            {
                UserId = seedInfo.UserId.ToString(),
                seedInfo.Info.Flags,
                seedInfo.Info.Url,
                seedInfo.Info.Version,
                seedInfo.Info.Seed,
                seedInfo.Info.Verification,
            });

            return new Response<int>(insertResponse, success: true);
        }
        catch (Exception ex)
        {
            return new Response<int>().InternalServerError(ex.Message);
        }
    }
}
