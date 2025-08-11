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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createRequest">A request detailing the required information for the race</param>
        /// <returns>the internal id of the newly created race</returns>
        /// <response code="200">In theory, this should be a 201 created, but is not.</response>
        /// <response code="401">Requests without a valid api key get a 401 response</response>
        [ApiKey, HttpPost]
        public async Task<IActionResult> LogRaceCreatedAsync(CreateRaceRoom createRequest)
        {
            var insertResponse = await _raceRepository.CreateRaceAsync(createRequest);
            return insertResponse.GetRequestResponse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">The amount of records to skip. Combined with limit, lets you page through results</param>
        /// <param name="limit">The amount of records to return. Limited to 500</param>
        /// <param name="includeCancelled">By default, races with a status of 'cancelled' are excluded. set to true to also return these</param>
        /// <param name="description">A word or phrase used in the race's description. If you're looking for races for a specific racer, use the /racers endoints instead.</param>
        /// <param name="flagset">A portion of the flagset you want to search for. There are some oddities to this.</param>
        /// <returns>A collection of RaceDetail objects. Unrecorded races are returned first, then in descending order of the race's finish time</returns>
        /// <response code="200"></response>
        /// <response code="400">Returned when requesting too much data</response> 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrSlug">The internal id (int)for the race, or the name of the racetime room.</param>
        /// <returns>The detail for the specifc race, including entrants</returns>
        /// <response code="200"></response>
        /// <response code="404">Returned when no race can be found by the given id or name</response>
        [HttpGet("{idOrSlug}")]
        public async Task<ActionResult<RaceDetail>> GetRaceAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceAsync(idOrSlug);
            return raceResponse.GetRequestResponse();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrSlug">The internal id (int) for the race, or the name of the racetime room.</param>
        /// <returns>returns the entire html document to reconstruct the patch page.</returns>
        /// <response code="200"></response>
        /// <response code="404">Returned when no race can be found by the given id or name</response>
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
