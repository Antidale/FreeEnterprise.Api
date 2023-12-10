using Dapper;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Repositories
{
    public class BattleLocationsRepository(IConnectionProvider connectionProvider) : IBattleLocationsRepository
	{
		private readonly IConnectionProvider _connectionProvider = connectionProvider;

        public async Task<IEnumerable<NameWithId>> GetBattleLocationsAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<NameWithId>(
                    "Select id as Id, battle_location as Name from locations.boss_fights;"
                );
			};
		}
	}
}
