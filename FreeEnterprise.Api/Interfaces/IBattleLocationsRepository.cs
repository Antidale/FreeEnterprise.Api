using FeInfo.Common.DTOs;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IBattleLocationsRepository
	{
		Task<IEnumerable<NameWithId>> GetBattleLocationsAsync();
	}
}
