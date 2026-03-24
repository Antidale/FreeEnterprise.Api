using System;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces;

public interface IRaceEntrantRepository
{
    Task InsertRaceEntrants(List<CreateRaceEntrantModel> entrants);
}
