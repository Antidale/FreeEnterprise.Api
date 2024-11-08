using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntrantController(IEntrantRepository entrantRepository) : ControllerBase
{
    [ApiKey]
    [HttpPut("UpdatePronouns")]
    public async Task<ActionResult> UpdatePronouns(UpdatePronouns updatePronouns)
    {
        var response = await entrantRepository.UpdatePronounsAsync(updatePronouns);
        return response.GetRequestResponse();
    }
}