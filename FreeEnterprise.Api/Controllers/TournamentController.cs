using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class TournamentController(ITournamentRepository tournamentRepository) : ControllerBase
	{
		private readonly ITournamentRepository _tournamentRepository = tournamentRepository;

        [HttpGet]
		public async Task<ActionResult<List<TournamentRegistration>>> GetTournamentSummaries()
		{
            var response = await _tournamentRepository.GetTournamentSummariesAsync();
            return response.GetRequestResponse();
		}

        [ApiKey]
        [HttpPost]
		public async Task<ActionResult> Create(CreateTournament createTournament)
		{
			var result = await _tournamentRepository.CreateTournamentAsync(createTournament);
			return result.GetRequestResponse();
		}

        [ApiKey]
        [HttpPatch("UpdateRegistrationWindow")]
		public async Task<ActionResult> UpdateRegistrationWindow(ChangeRegistrationPeriod request)
		{
            var result = await _tournamentRepository.UpdateRegistrationWindow(request);
            return result.GetRequestResponse();
        }

		[ApiKey]
        [HttpPost("Register")]
		public async Task<ActionResult<ChangeRegistrationResponse>> RegisterPlayer(ChangeRegistration request)
		{
            var result = await _tournamentRepository.RegisterPlayerAsync(request);
            return result.GetRequestResponse();
        }

        [ApiKey]
        [HttpPost("Drop")]
        public async Task<ActionResult<ChangeRegistrationResponse>> DropPlayer(ChangeRegistration request)
        {
            var result = await _tournamentRepository.DropPlayerAsync(request);
			return result.GetRequestResponse();
        }
    }
}
