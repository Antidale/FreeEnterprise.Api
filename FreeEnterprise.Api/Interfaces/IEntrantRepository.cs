using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces;

public interface IEntrantRepository
{
    Task<Response> UpdatePronounsAsync(UpdatePronouns updatePronouns);
}