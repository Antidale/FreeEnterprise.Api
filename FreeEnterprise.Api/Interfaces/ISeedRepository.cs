using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces;

public interface ISeedRepository
{
    Task<Response<int>> SaveSeedRolledAsync(LogSeedRoled seedInfo);
}
