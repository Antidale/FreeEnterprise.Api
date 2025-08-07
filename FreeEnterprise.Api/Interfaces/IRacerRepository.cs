using System;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces;

public interface IRacerRepository
{
    Task<Response<List<Racer>>> GetRacersAsync(int offset, int limit, string? idOrName);
    Task<Response<Racer>> GetRacerAsync(string idOrName);
}
