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

        /// <summary>
        ///
        /// </summary>
        /// <param name="logRequest">The listed userId property of the request should represent the user's discord id.</param>
        /// <returns></returns>
        /// <response code="200">In theory, this should be a 201 created, but it is not.</response>
        /// <response code="401">Requests without a valid api key get a 401 response</response>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset">Skips this amount of records in your data set. Combined with limit, used with to page through results.</param>
        /// <param name="limit">The maximum amount of SeedDetail objects to return from your query. Currently no limit is set, but this can change without warning in the future. Combine with offset to page through results</param>
        /// <param name="flagSearch">Search for a flagset that contains a desired flag. Can be a bit wonky.</param>
        /// <param name="seedString">search for an exact match of the seed value.</param>
        /// <param name="binaryFlags">search for an exact match of the binary flag string.</param>
        /// <returns></returns>
        /// <response code="200"></response>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="seedId">The specific SeedDetail to get, if you've previously stored the id.</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Returned when no seed can be found by the given id</response>
        [HttpGet("{seedId:int}")]
        public async Task<ActionResult<SeedDetail>> GetSeedAsync(int seedId)
        {
            var response = await _seedRepository.GetSeedByIdAsync(seedId);

            return response.GetRequestResponse();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seedId">The id of the specific seed to get the patch page for, if you've previously stored the id.</param>
        /// <returns>the actual page page html. suitable for using in an iFrame, or for saving to your own system. This api doesn't not return it as a page.</returns>
        /// <response code="200"></response>
        /// <response code="404">Returned when no seed can be found by the given id</response>
        [HttpGet("{seedId:int}/html")]
        public async Task<ActionResult<string>> GetSeedHtmlAsync(int seedId)
        {
            var response = await _seedRepository.GetPatchBySeedIdAsync(seedId);

            return response.GetRequestResponse();
        }
    }
}
