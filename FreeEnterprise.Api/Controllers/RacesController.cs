using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController(IRaceRespository raceRespository, IMemoryCache memoryCache) : ControllerBase
    {
        private readonly IRaceRespository _raceRepository = raceRespository;

        [ApiKey, HttpPost]
        public async Task<IActionResult> LogRaceCreatedAsync(CreateRaceRoom createRequest)
        {
            var insertResponse = await _raceRepository.CreateRaceAsync(createRequest);
            return insertResponse.GetRequestResponse();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceDetail>>> GetRacesAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] bool includeCancelled = false,
            [FromQuery] string? description = null,
            [FromQuery] string? flagset = null

        )
        {
            if (limit > 500)
            {
                return BadRequest("Cannot ask for more than 500 races");
            }

            var cacheKey = $"Races_o{offset}_l{limit}_d{description}_f{flagset}_{includeCancelled}";

            if (memoryCache.TryGetValue<IEnumerable<RaceDetail>>(cacheKey, out var raceDetails) && raceDetails is not null)
            {
                return Classes.Response.SetSuccess(raceDetails).GetRequestResponse();
            }

            var getResponse = await _raceRepository.GetRacesAsync(offset, limit, includeCancelled, description, flagset);

            memoryCache.SetCache(cacheKey, getResponse.Data);
            return getResponse.GetRequestResponse();
        }

        [HttpGet("{idOrSlug}")]
        public async Task<ActionResult<RaceDetail>> GetRaceAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceAsync(idOrSlug);
            return raceResponse.GetRequestResponse();
        }

        [HttpGet("{idOrSlug}/seed")]
        public async Task<ActionResult<string>> GetRaceSeedAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceSeedHtmlAsync(idOrSlug);
            return raceResponse.GetRequestResponse();
        }

        // [ApiKey, HttpPost]
        // public async Task<ActionResult> JoinRaceAsync(JoinRaceRequest joinRequest)
        // {
        //     var joinResponse = await _raceRepository.JoinRaceAsync(joinRequest);
        //     return joinResponse.GetRequestResponse();
        // }
    }
}
