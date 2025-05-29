using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
