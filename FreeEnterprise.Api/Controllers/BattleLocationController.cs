using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BattleLocationController(IBattleLocationsRepository battleLocationsRepository) : ControllerBase
	{
		private readonly IBattleLocationsRepository _battleLocationsRepository = battleLocationsRepository;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<NameWithId>>> Get()
		{
			var locations = await _battleLocationsRepository.GetBattleLocationsAsync();
			return Ok(locations);
		}
	}
}
