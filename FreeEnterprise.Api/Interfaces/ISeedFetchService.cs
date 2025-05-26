using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces;

public interface ISeedFetchService
{
    Task<Response<string>> GetPatchHtmlAsync(SeedInformation seedInfo);
}
