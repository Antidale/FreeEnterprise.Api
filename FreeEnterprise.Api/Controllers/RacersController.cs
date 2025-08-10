using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;


namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacersController(IRacerRepository racerRepository, IMemoryCache memoryCache) : ControllerBase
    {
        private readonly IRacerRepository _racerRepository = racerRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Racer>>> GetRacersAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] string? name = null
        )
        {
            var cacheKey = $"Races_o{offset}_l{limit}_n{name}";

            if (memoryCache.TryGetValue<IEnumerable<Racer>>(cacheKey, out var raceDetails) && raceDetails is not null)
            {
                return Classes.Response.SetSuccess(raceDetails).GetRequestResponse();
            }

            var getResponse = await _racerRepository.GetRacersAsync(offset, limit, name);

            memoryCache.SetCache(cacheKey, getResponse.Data);

            return getResponse.GetRequestResponse();
        }

        [HttpGet("{idOrName}")]
        public async Task<ActionResult<Racer>> GetRacerAsync(
            string idOrName
        )
        {
            var getResponse = await _racerRepository.GetRacerAsync(idOrName);
            return getResponse.GetRequestResponse();
        }

        [HttpGet("{idOrName}/races")]
        public async Task<ActionResult<IEnumerable<RaceDetail>>> GetRacerRacesAsync(
            string idOrName,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20
        )
        {
            var cacheKey = $"Racer_n{idOrName}_o{offset}_l{limit}";
            if (memoryCache.TryGetValue<IEnumerable<RaceDetail>>(cacheKey, out var raceDetails) && raceDetails is not null)
            {
                return Classes.Response.SetSuccess(raceDetails).GetRequestResponse();
            }
            var response = await _racerRepository.GetRacesForRacerAsync(idOrName, offset, limit);

            memoryCache.SetCache(cacheKey, response.Data);

            return response.GetRequestResponse();
        }

        [HttpGet("{idOrName}/head-to-head/{opponentIdOrName}")]
        public async Task<ActionResult<IEnumerable<RaceDetail>>> GetHeadToHeadAsync(
            string idOrName,
            string opponentIdOrName
        )
        {
            var cacheKey = $"Racer_h2h_n{idOrName}_opp{opponentIdOrName}";

            if (memoryCache.TryGetValue<IEnumerable<Racer>>(cacheKey, out var raceDetails) && raceDetails is not null)
            {
                return Classes.Response.SetSuccess(raceDetails).GetRequestResponse();
            }

            var response = await _racerRepository.GetHeadToHeadAsync(idOrName, opponentIdOrName);

            memoryCache.SetCache(cacheKey, response.Data);

            return response.GetRequestResponse();
        }
    }
}
