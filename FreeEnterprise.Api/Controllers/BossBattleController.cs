using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BossBattleController(IBossBattlesRepository bossBattlesRepository) : ControllerBase
	{
		private readonly IBossBattlesRepository _bossBattlesRepository = bossBattlesRepository;

        [HttpGet]
		public async Task<ActionResult<IEnumerable<BossBattle>>> Get()
		{
			var locations = await _bossBattlesRepository.GetBossBattlesAsync();
			return Ok(locations);
		}
	}
}
