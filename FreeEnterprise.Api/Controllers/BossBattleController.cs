using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BossBattleController : ControllerBase
	{
		private readonly IBossBattlesRepository _bossBattlesRepository;
		public BossBattleController(IBossBattlesRepository bossBattlesRepository)
		{
			_bossBattlesRepository = bossBattlesRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BossBattle>>> Get()
		{
			var locations = await _bossBattlesRepository.GetBossBattlesAsync();
			return Ok(locations);
		}
	}
}
