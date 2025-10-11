using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;


namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacersController(IRacerRepository racerRepository, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly IRacerRepository _racerRepository = racerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Results are cached for 5 minutes, based on the combination of the offset, limit, and name query parameters
        /// </remarks>
        /// <param name="offset">used with limit to accomplish paging through results</param>
        /// <param name="limit">used with offset to accomplish paging through results. Currently no maximum value, but there might be a limit imposed at some point in the future</param>
        /// <param name="name">The name of the racer, currently looks at either their racetime name or the name of the twitch account linked to their discord account. search is case insensitive, and all names starting with your search string are returned, within the offset and limit parameters. </param>
        /// <returns>A collection of Racer objects sorted by RacetimeName</returns>
        /// <response code="200"></response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrName">preferably, use the user's id (not name) from racetime.gg. The user's name at racetime and an associated twitch account can match, but might result in a 409 if multiple different users share that information</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Returned when no user can be found by the given id or name</response>
        /// <response code="409">Returned when more than one user can be found by the provided name</response>
        [HttpGet("{idOrName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Racer>> GetRacerAsync(
            string idOrName
        )
        {
            var getResponse = await _racerRepository.GetRacerAsync(idOrName);
            return getResponse.GetRequestResponse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Results are cached for five minutes, based on the combination of limit and offset.</remarks>
        /// <param name="idOrName">the racetime id of the user, or their ractime mae or the name of the twitch account registered to their racetime account</param>
        /// <param name="offset"></param>
        /// <param name="limit">The amount of <see cref="RaceDetail"/>RaceDetails to return in this call. Combine with offset to page through records</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="400">Returned when requesting too much data</response>
        [HttpGet("{idOrName}/races")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RaceDetail>>> GetRacerRacesAsync(
            string idOrName,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20
        )
        {
            if (limit > 300)
            {
                return BadRequest("Cannot request more than 300 races for a racer");
            }

            var cacheKey = $"Racer_n{idOrName}_o{offset}_l{limit}";
            if (memoryCache.TryGetValue<IEnumerable<RaceDetail>>(cacheKey, out var raceDetails) && raceDetails is not null)
            {
                return Classes.Response.SetSuccess(raceDetails).GetRequestResponse();
            }
            var response = await _racerRepository.GetRacesForRacerAsync(idOrName, offset, limit);

            memoryCache.SetCache(cacheKey, response.Data);

            return response.GetRequestResponse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Provides all of the races both runners have raced in</remarks>
        /// <param name="idOrName">Either the ractime.gg id, or the racer's display name for either rt.gg or twitch</param>
        /// <param name="opponentIdOrName">Either the ractime.gg id, or the racer's display name for either rt.gg or twitch</param>
        /// <returns>A collection of RaceDetail objects. The Entrants property of those races will bel limited to just the two requested racers.</returns>
        /// <response code="200"></response>
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

        [HttpGet("racetime-search")]
        public async Task<ActionResult> SearchRacetimeUser()
        {
            List<string> validKeys = ["name", "discriminator", "term"];
            var validPairs = HttpContext.Request.Query.Where(x => validKeys.Contains(x.Key)).Select(x => $"{x.Key}={x.Value}");
            if (!validPairs.Any())
            {
                return BadRequest();
            }

            var querystring = $"?{string.Join('&', validPairs)}";
            if (memoryCache.TryGetValue<string>(querystring, out var cacheResponse) && cacheResponse is not null)
            {
                return Ok(cacheResponse);
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://racetime.gg/user/search{querystring}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                memoryCache.SetCache(querystring, result);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
