using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Requests;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IBossStatsRepository
	{
		Task<IEnumerable<BossStat>> SearchAsync(BossStatsSearchRequest request);
	}
}
