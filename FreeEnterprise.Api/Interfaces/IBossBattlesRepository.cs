using FeInfo.Common.DTOs;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IBossBattlesRepository
	{
		Task<IEnumerable<NameWithId>> GetBossBattlesAsync();
	}
}
