using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Dapper;

namespace FreeEnterprise.Api.Repositories
{
	public class BattleLocationsRepository : IBattleLocationsRepository
	{
		private readonly IConnectionProvider _connectionProvider;
		public BattleLocationsRepository(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}
		public async Task<IEnumerable<BattleLocation>> GetBattleLocationsAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<BattleLocation>(
					"Select battle_location as Name, id as Id from locations.boss_fights;"
				);
			};
		}
	}
}
