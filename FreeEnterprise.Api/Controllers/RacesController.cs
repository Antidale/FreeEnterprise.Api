using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
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
            return insertResponse.GetResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetRacesAsync(
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
        public async Task<IActionResult> GetRaceAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceAsync(idOrSlug);
            return raceResponse.GetResult();
        }

        [HttpGet("{idOrSlug}/seed")]
        public async Task<IActionResult> GetRaceSeedAsync(string idOrSlug)
        {
            var raceResponse = await _raceRepository.GetRaceSeedHtmlAsync(idOrSlug);
            return raceResponse.GetResult();
        }

        // [ApiKey, HttpPost]
        // public async Task<IActionResult> JoinRaceAsync(JoinRaceRequest joinRequest)
        // {
        //     var joinResponse = await _raceRepository.JoinRaceAsync(joinRequest);
        //     return joinResponse.GetRequestResponse();
        // }
    }
}
