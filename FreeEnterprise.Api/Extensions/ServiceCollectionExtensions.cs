using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Repositories;

namespace FreeEnterprise.Api.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection collection)
    {
        public IServiceCollection AddRepositories()
        {
            collection.AddSingleton<IBattleLocationsRepository, BattleLocationsRepository>();
            collection.AddSingleton<IBossBattlesRepository, BossBattlesRepository>();
            collection.AddSingleton<IBossStatsRepository, BossStatsRepository>();
            collection.AddSingleton<IEquipmentRepository, EquipmentRepository>();
            collection.AddSingleton<ITournamentRepository, TournamentRepository>();
            collection.AddSingleton<IEntrantRepository, EntrantRepository>();
            collection.AddSingleton<IGuidesRepository, GuidesRepository>();
            collection.AddSingleton<ISeedRepository, SeedRepository>();
            collection.AddSingleton<IRaceRespository, RaceRepository>();
            collection.AddSingleton<IRacerRepository, RacerRepository>();
            collection.AddSingleton<IRaceEntrantRepository, RaceEntrantRepository>();

            return collection;
        }
    }
}
