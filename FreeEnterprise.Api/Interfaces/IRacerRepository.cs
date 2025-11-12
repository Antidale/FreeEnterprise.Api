using FreeEnterprise.Api.Classes;
namespace FreeEnterprise.Api.Interfaces;

public interface IRacerRepository
{
    Task<Response<IEnumerable<Racer>>> GetRacersAsync(int offset, int limit, string? idOrName);
    Task<Response<Racer>> GetRacerAsync(string idOrName);
    Task<Response<IEnumerable<RaceDetail>>> GetRacesForRacerAsync(string idOrName, int offset, int limit);
    Task<Response<IEnumerable<RaceDetail>>> GetHeadToHeadAsync(string idOrName, string opponentIdOrName);
}
