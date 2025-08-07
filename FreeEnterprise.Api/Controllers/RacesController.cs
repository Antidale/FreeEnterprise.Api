using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

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
            return insertResponse.GetResult();
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
            return getResponse.GetResult();
        }

        [HttpGet("{idOrSlug}")]
        public async Task<ActionResult<RaceDetail>> GetRaceAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceAsync(idOrSlug);
            return raceResponse.GetResult();
        }

        [HttpGet("{idOrSlug}/seed")]
        public async Task<ActionResult<string>> GetRaceSeedAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceSeedHtmlAsync(idOrSlug);
            return raceResponse.GetResult();
        }

        [HttpGet("{idOrSlug}/entrants")]
        public async Task<ActionResult<string>> GetRaceEntrantsAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceSeedHtmlAsync(idOrSlug);
            return raceResponse.GetResult();
        }

        // [ApiKey, HttpPost]
        // public async Task<ActionResult> JoinRaceAsync(JoinRaceRequest joinRequest)
        // {
        //     var joinResponse = await _raceRepository.JoinRaceAsync(joinRequest);
        //     return joinResponse.GetRequestResponse();
        // }
    }
}
