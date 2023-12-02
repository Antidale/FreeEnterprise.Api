using Dapper;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Repositories
{
    public class BattleLocationsRepository : IBattleLocationsRepository
	{
		private readonly IConnectionProvider _connectionProvider;
		public BattleLocationsRepository(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}
		public async Task<IEnumerable<NameWithId>> GetBattleLocationsAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<NameWithId>(
					"Select battle_location as Name, id as Id from locations.boss_fights;"
				);
			};
		}
	}
}
