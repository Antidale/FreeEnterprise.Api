using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController(IRaceRepository raceRespository) : ControllerBase
    {
        private readonly IRaceRepository _raceRepository = raceRespository;

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
