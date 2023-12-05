using Dapper;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Repositories
{
    public class BossBattlesRepository : IBossBattlesRepository
	{
		private readonly IConnectionProvider _connectionProvider;

		public BossBattlesRepository(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public async Task<IEnumerable<NameWithId>> GetBossBattlesAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<NameWithId>(
                    "Select id as Id, battle as Name from encounters.boss_fights;"
                );
			};
		}
	}
}
