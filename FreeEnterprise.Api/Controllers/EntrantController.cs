using FeInfo.Common.Requests;
using FreeEnterprise.Api.Attributes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntrantController(IEntrantRepository entrantRepository) : ControllerBase
{
    [ApiKey]
    [HttpPatch("UpdatePronouns")]
    public async Task<ActionResult> UpdatePronouns(UpdatePronouns updatePronouns)
    {
        var response = await entrantRepository.UpdatePronounsAsync(updatePronouns);
        return response.GetRequestResponse();
    }

    [ApiKey]
    [HttpPatch("UpdateAlias")]
    public async Task<ActionResult> UpdateAlias(UpdateAlias updateAlias)
    {
        var response = await entrantRepository.UpdateAliasAsync(updateAlias);
        return response.GetRequestResponse();
    }
    
    [ApiKey]
    [HttpPatch("UpdateTwitch")]
    public async Task<ActionResult> UpdateTwitch(UpdateTwitch updateTwitch)
    {
        var response = await entrantRepository.UpdateTwitchAsync(updateTwitch);
        return response.GetRequestResponse();
    }
}