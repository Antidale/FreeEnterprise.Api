using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    public class GuideController(IGuidesRepository guidesRepository) : ControllerBase
    {
        private readonly IGuidesRepository _guidesRepository = guidesRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guide>>> Search([FromQuery] string searchText, [FromQuery] int? limit = null)
        {
            var result = await _guidesRepository.GetGuidesAsync(searchText, limit);
            return result.GetRequestResponse();
        }

    }
}
