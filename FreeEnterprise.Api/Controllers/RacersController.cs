using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;


namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacersController(IRacerRepository racerRepository) : ControllerBase
    {
        private readonly IRacerRepository _racerRepository = racerRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Racer>>> GetRacersAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] string? name = null
        )
        {
            var getResponse = await _racerRepository.GetRacersAsync(offset, limit, name);
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
            var response = await _racerRepository.GetRacesForRacerAsync(idOrName, offset, limit);
            return response.GetRequestResponse();
        }

        [HttpGet("{idOrName}/head-to-head/{opponentIdOrName}")]
        public async Task<ActionResult<IEnumerable<RaceDetail>>> GetHeadToHeadAsync(
            string idOrName,
            string opponentIdOrName
        )
        {
            var response = await _racerRepository.GetHeadToHeadAsync(idOrName, opponentIdOrName);
            return response.GetRequestResponse();
        }
    }
}
