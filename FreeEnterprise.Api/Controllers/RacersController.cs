using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetRacersAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] string? name = null
        )
        {
            return Ok();
        }

        [HttpGet("{idOrName}")]
        public async Task<ActionResult> GetRacerAsync(
            string idOrName
        )
        {
            return Ok();
        }

        [HttpGet("{idOrName}/races")]
        public async Task<ActionResult> GetRacerRacesAsync(
            string idOrName
        )
        {
            return Ok();
        }

        [HttpGet("{idOrName}/head-to-head/{oponnentIdOrName}")]
        public async Task<ActionResult> GetHeadToHeadAsync(
            string idOrName,
            string oponnentIdOrName
        )
        {
            return Ok();
        }
    }
}
