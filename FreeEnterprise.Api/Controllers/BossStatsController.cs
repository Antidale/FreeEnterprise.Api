using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Requests;

namespace FreeEnterprise.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BossStatsController : ControllerBase
	{
		private readonly IBossStatsRepository _bossStatsRepository;
		public BossStatsController(IBossStatsRepository bossStatsRepository)
		{
			_bossStatsRepository = bossStatsRepository;
		}

		[HttpPost]
		public async Task<ActionResult<IEnumerable<BossBattle>>> Search(BossStatsSearchRequest request)
		{
			if (request.BattleId == 0 && request.LocationId == 0)
				return BadRequest();

			return Ok(await _bossStatsRepository.SearchAsync(request));
		}
	}
}
