using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BattleLocationController : ControllerBase
	{
		private readonly IBattleLocationsRepository _battleLocationsRepository;
		public BattleLocationController(IBattleLocationsRepository battleLocationsRepository)
		{
			_battleLocationsRepository = battleLocationsRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BattleLocation>>> Get()
		{
			var locations = await _battleLocationsRepository.GetBattleLocationsAsync();
			return Ok(locations);
		}
	}
}
