using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Classes;
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
                return insertResponse.GetRequestResponse();
            }

            var patchhtmlResponse = await _seedFetchService.GetPatchHtmlAsync(logRequest.Info);
            if (patchhtmlResponse.Success && patchhtmlResponse.Data is not null)
            {
                await _seedRepository.SavePatchHtml(insertResponse.Data, patchhtmlResponse.Data);
            }

            //Intentionally swallowing any errors with saving the patch page
            return insertResponse.GetRequestResponse();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeedDetail>>> GetSeedsAsync
        (
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] string? flagSearch = null,
            [FromQuery] string? seedString = null,
            [FromQuery] string? binaryFlags = null
        )
        {
            var response = await _seedRepository.SearchSeedDetails(offset: offset, limit: limit, flagset: flagSearch, seedValue: seedString, binaryFlags: binaryFlags);

            return response.GetRequestResponse();
        }

        [HttpGet("{seedId:int}")]
        public async Task<ActionResult<SeedDetail>> GetSeedAsync(int seedId)
        {
            var response = await _seedRepository.GetSeedByIdAsync(seedId);

            return response.GetRequestResponse();
        }

        [HttpGet("{seedId:int}/html")]
        public async Task<ActionResult<string>> GetSeedHtmlAsync(int seedId)
        {
            var response = await _seedRepository.GetPatchBySeedIdAsync(seedId);

            return response.GetRequestResponse();
        }
    }
}
