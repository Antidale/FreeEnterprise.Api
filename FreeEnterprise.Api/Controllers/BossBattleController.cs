using System;
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
