using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Providers;
using System.Collections.Generic;
using FreeEnterprise.Api.Models;
using Dapper;
using System.Threading.Tasks;
using System;

namespace FreeEnterprise.Api.Repositories
{
	public class EquipmentRepository : IEquipmentRepository
	{
		private readonly IConnectionProvider _connectionProvider;

		public EquipmentRepository(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public async Task<IEnumerable<Equipment>> GetEquipmentAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<Equipment>
				(
					$@"select
								id
							, item_name as {nameof(Equipment.Name)}
							, equipment_type as {nameof(Equipment.EquipmentType)}
							, category as {nameof(Equipment.Category)}
							, str as {nameof(Equipment.Str)}
							, agi as {nameof(Equipment.Agi)}
							, vit as {nameof(Equipment.Vit)}
							, wil as {nameof(Equipment.Wil)}
							, wis as {nameof(Equipment.Wis)}
							, strong_vs as {nameof(Equipment.StrongAgainst)}
							, magnetic as {nameof(Equipment.Magnetic)}
							, can_equip as {nameof(Equipment.CanEquip)}
						from equipment.base"
				);
			}
		}

		public Task<IEnumerable<Armor>> GetArmorAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Weapon>> GetWeaponsAsync()
		{
			throw new NotImplementedException();
		}
	}
}
