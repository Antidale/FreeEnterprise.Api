using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BossStatsController(IBossStatsRepository bossStatsRepository) : ControllerBase
	{
		private readonly IBossStatsRepository _bossStatsRepository = bossStatsRepository;

        [HttpPost]
		public async Task<ActionResult<IEnumerable<NameWithId>>> Search(BossStatsSearchRequest request)
		{
			if (request.BattleId == 0 && request.LocationId == 0)
				return BadRequest();

			return Ok(await _bossStatsRepository.SearchAsync(request));
		}
	}
}
