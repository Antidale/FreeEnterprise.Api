using System;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces;

public interface IRaceRespository
{
    Task<Response<int>> CreateRaceAsync(CreateRaceRoom createRequest);
    Task<Response<IEnumerable<RaceDetail>>> GetRacesAsync();
    Task<Response> JoinRaceAsync(JoinRaceRequest joinRaceRequest);
}
