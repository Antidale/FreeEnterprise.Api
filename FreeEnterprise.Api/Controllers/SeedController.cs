using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(ISeedRepository seedRepository, ISeedFetchService seedFetchService) : ControllerBase
    {
        private readonly ISeedRepository _seedRepository = seedRepository;
        private readonly ISeedFetchService _seedFetchService = seedFetchService;

        [ApiKey, HttpPost]
        public async Task<ActionResult> LogSeed(LogSeedRoled logRequest)
        {
            var insertResponse = await _seedRepository.SaveSeedRolledAsync(logRequest);
            if (!insertResponse.Success)
            {
                return insertResponse.GetResult();
            }

            var patchhtmlResponse = await _seedFetchService.GetPatchHtmlAsync(logRequest.Info);
            if (patchhtmlResponse.Success && patchhtmlResponse.Data is not null)
            {
                await _seedRepository.SavePatchHtml(insertResponse.Data, patchhtmlResponse.Data);
            }

            //Intentionally swallowing any errors with saving the patch page
            return insertResponse.GetResult();
        }

        [HttpGet]
        public async Task<ActionResult> GetSeedsAsync
        (
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] string? flags = null
        )
        {
            return Ok();
        }

        [HttpGet("{seedId:int}")]
        public async Task<ActionResult> GetSeedAsync(int seedId)
        {
            return Ok();
        }

        [HttpGet("{seedId:int/html}")]
        public async Task<ActionResult> GetSeedHtmlAsync(int seedId)
        {
            return Ok();
        }
    }
}
