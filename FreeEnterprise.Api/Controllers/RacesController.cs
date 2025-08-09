using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;


namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController(IRaceRespository raceRespository) : ControllerBase
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
            [FromQuery] string? description = null,
            [FromQuery] string? flagset = null
        )
        {
            //TODO: add min/max values for limit (5 || 50?)
            var getResponse = await _raceRepository.GetRacesAsync(offset, limit, description, flagset);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrSlug">Use either the RaceId or the RoomName from a RaceDetail</param>
        /// <returns>A List of RaceEntrant</returns>
        /// <response code="200">Returns the list of entrants if the race and entrants exist. The list will return empty if either the idOrSlug is not valid, or if there are not entrants.</response>
        /// <response code="500">Returns 500 when any execption happens</response>
        [HttpGet("{idOrSlug}/entrants")]
        public async Task<ActionResult<IEnumerable<RaceEntrant>>> GetRaceEntrantsAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceEntrantsAsync(idOrSlug);
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
