using FeInfo.Common.DTOs;
using FeInfo.Common.Enums;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IGuidesRepository
    {
        Task<Response<IEnumerable<Guide>>> GetGuidesAsync(string searchText, int? limit = null);
        Task<Response<BossStrategy>> GetBossStrategyAsync(BossName bossName);
    }
}
