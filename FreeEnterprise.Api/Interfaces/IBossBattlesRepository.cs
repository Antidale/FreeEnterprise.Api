using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IBossBattlesRepository
	{
		Task<IEnumerable<BossBattle>> GetBossBattlesAsync();
	}
}
