using FreeEnterprise.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeEnterprise.Api.Interfaces
{
	public interface IBossBattlesRepository
	{
		Task<IEnumerable<BossBattle>> GetBossBattlesAsync();
	}
}
