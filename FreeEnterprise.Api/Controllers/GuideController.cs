using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
    public class GuideController(IGuidesRepository guidesRepository) : ControllerBase
    {
        private readonly IGuidesRepository _guidesRepository = guidesRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guide>>> Search([FromQuery] string searchText)
        {
            var result = await _guidesRepository.GetGuidesAsync(searchText);
            return result.GetRequestResponse();
        }

    }
}
