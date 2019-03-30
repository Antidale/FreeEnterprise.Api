using System.Collections.Generic;
using System.Threading.Tasks;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Requests;

namespace FreeEnterprise.Api.Interfaces
{
	public interface IBossStatsRepository
	{
		Task<IEnumerable<BossStat>> SearchAsync(BossStatsSearchRequest request);
	}
}
