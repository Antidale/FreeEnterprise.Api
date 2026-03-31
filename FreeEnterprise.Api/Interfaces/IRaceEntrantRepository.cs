using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces;

public interface IRaceEntrantRepository
{
    Task<Response> InsertRaceEntrants(List<CreateRaceEntrantModel> entrants);
}
