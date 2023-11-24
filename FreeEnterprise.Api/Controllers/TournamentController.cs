using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class TournamentController : ControllerBase
	{
		private readonly ITournamentRepository _tournamentRepository;
		public TournamentController(TournamentRepository tournamentRepository)
		{
			_tournamentRepository = tournamentRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetEntrants()
		{
			
			return Ok();
		}

		[HttpPost]
		public async Task<>
	}
}
