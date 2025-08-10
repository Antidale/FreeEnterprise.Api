using FeInfo.Common.DTOs;
using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Microsoft.Extensions.Caching.Memory;


namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController(ITournamentRepository tournamentRepository, IMemoryCache memoryCache) : ControllerBase
    {
        private readonly ITournamentRepository _tournamentRepository = tournamentRepository;

        [HttpGet]
        public async Task<ActionResult<List<TournamentSummary>>> GetTournamentSummaries()
        {
            const string cacheKey = "Tournaments";
            if (memoryCache.TryGetValue(cacheKey, out List<TournamentSummary>? tournaments) && tournaments is not null)
            {
                return Classes.Response.SetSuccess(tournaments).GetRequestResponse();
            }
            var response = await _tournamentRepository.GetTournamentSummariesAsync();

            memoryCache.SetCache(cacheKey, response.Data, TimeSpan.FromMinutes(30));

            return response.GetRequestResponse();
        }

        [HttpGet("{id:int}/registrants")]
        public async Task<ActionResult<List<TournamentRegistration>>> GetTournamentRegistrants(int id)
        {
            string cacheKey = $"Tournaments_{id}";
            if (memoryCache.TryGetValue(cacheKey, out List<TournamentRegistrant>? registrants) && registrants is not null)
            {
                return Classes.Response.SetSuccess(registrants).GetRequestResponse();
            }

            var response = await _tournamentRepository.GetTournamentRegistrantsAsync(id);
            memoryCache.Set(cacheKey, response.Data,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
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
