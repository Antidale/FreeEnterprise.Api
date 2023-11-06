using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IBattleLocationsRepository
	{
		Task<IEnumerable<BattleLocation>> GetBattleLocationsAsync();
	}
}
