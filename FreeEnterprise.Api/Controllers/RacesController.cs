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

        // [ApiKey, HttpPost]
        // public async Task<IActionResult> JoinRaceAsync(JoinRaceRequest joinRequest)
        // {
        //     var joinResponse = await _raceRepository.JoinRaceAsync(joinRequest);
        //     return joinResponse.GetRequestResponse();
        // }
    }


}
