using Dapper;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Requests;

namespace FreeEnterprise.Api.Repositories
{
    public class BossStatsRepository : IBossStatsRepository
	{
		private readonly IConnectionProvider _connectionProvider;

		public BossStatsRepository(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public async Task<IEnumerable<BossStat>> SearchAsync(BossStatsSearchRequest request)
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<BossStat>(
					$@"select
							  s.id
							, e.Battle
							, l.battle_location as {nameof(BossStat.Location)}
							, s.enemy
							, s.Level
							, s.hit_points as {nameof(BossStat.HitPoints)}
							, s.experience_points as {nameof(BossStat.ExperiencePoints)}
							, s.gil
							, s.attack_multiplier as {nameof(BossStat.AttackMultiplier)}
							, s.attack_percent as {nameof(BossStat.AttackPercent)}
							, s.attack_power as {nameof(BossStat.AttackPower)}
							, s.defense_multiplier as {nameof(BossStat.DefenseMultiplier)}
							, s.evade
							, s.defense
							, s.magic_defense_multiplier as {nameof(BossStat.MagicDefenseMultiplier)}
							, s.magic_evade as {nameof(BossStat.MagicEvade)}
							, s.min_speed as {nameof(BossStat.MinSpeed)}
							, s.max_speed as {nameof(BossStat.MaxSpeed)}
							, s.spell_power as {nameof(BossStat.SpellPower)}
							, s.script_values as {nameof(BossStat.ScriptValues)}
						from stats.bosses s
						join encounters.boss_fights e
							on s.battle_id = e.id
						join locations.boss_fights l
							on s.location_id = l.id
						where (location_id = @locationId or @locationId = 0)
						and (battle_id = @battleId or @battleId = 0);",
					new { locationId = request.LocationId, battleId = request.BattleId }
				);
			}
		}
	}
}
