using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Interfaces;

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
			locations = locations.Concat(new List<BossBattle> { new BossBattle { Id = 0, Name = "All" } }).OrderBy(x => x.Name);
			return Ok(locations);
		}
	}
}
