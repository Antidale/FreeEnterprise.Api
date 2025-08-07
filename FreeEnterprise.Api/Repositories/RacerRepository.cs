using System;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class RacerRepository(IConnectionProvider connectionProvider, ILogger<RacerRepository> logger) : IRacerRepository
{
    private readonly IConnectionProvider _connectionPrivoder = connectionProvider;

    public Task<Response<Racer>> GetRacerAsync(string idOrName)
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<Racer>>> GetRacersAsync(int offset, int limit, string? idOrName)
    {
        throw new NotImplementedException();
    }
}
