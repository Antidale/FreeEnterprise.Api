using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Dapper;

namespace FreeEnterprise.Api.Repositories
{
	public class BossBattlesRepository : IBossBattlesRepository
	{
		private readonly IConnectionProvider _connectionProvider;

		public BossBattlesRepository(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public async Task<IEnumerable<BossBattle>> GetBossBattlesAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<BossBattle>(
					"Select battle as Name, id as Id from encounters.boss_fights;"
				);
			};
		}
	}
}
