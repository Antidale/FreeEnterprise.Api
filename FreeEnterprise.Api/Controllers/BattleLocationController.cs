using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Interfaces;

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
			locations = locations.Concat(new List<BattleLocation> { new BattleLocation { Id = 0, Name = "All" } }).OrderBy(x => x.Name);
			return Ok(locations);
		}
	}
}
