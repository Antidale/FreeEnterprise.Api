using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(ISeedRepository seedRepository) : ControllerBase
    {
        private readonly ISeedRepository _seedRepository = seedRepository;

        [ApiKey]
        [HttpPost]
        public async Task<ActionResult> LogSeedAsync(LogSeedRoled logRequest)
        {
            var insertResponse = await _seedRepository.SaveSeedRolledAsync(logRequest);

            return insertResponse.GetRequestResponse();
        }
    }
}
