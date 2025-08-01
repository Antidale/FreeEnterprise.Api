using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces;

public interface ISeedRepository
{
    Task<Response<string>> GetPatchBySeedIdAsync(int id);
    Task<Response<SeedDetail>> GetSeedByIdAsync(int id);
    Task<Response<int>> SaveSeedRolledAsync(LogSeedRoled seedInfo);
    Task<Response> SavePatchHtml(int savedSeedId, string html);
    Task<Response<IEnumerable<SeedDetail>>> SearchSeedDetails(int offset, int limit, string? flagset, string? binaryFlags, string? seedValue);
}
