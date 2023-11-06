using Dapper;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

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
							, item_name as {nameof(Armor.Name)}
							, equipment_type as {nameof(Armor.EquipmentType)}
							, category as {nameof(Armor.Category)}
							, str as {nameof(Armor.Str)}
							, agi as {nameof(Armor.Agi)}
							, vit as {nameof(Armor.Vit)}
							, wil as {nameof(Armor.Wil)}
							, wis as {nameof(Armor.Wis)}
							, strong_vs as {nameof(Armor.StrongAgainst)}
							, magnetic as {nameof(Armor.Magnetic)}
							, can_equip as {nameof(Armor.CanEquip)}
							, icon as {nameof(Armor.Icon)}
							, notes as {nameof(Armor.Notes)}
						from equipment.base"
				);
			}
		}

		public async Task<IEnumerable<Armor>> GetArmorAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<Armor>
				(
					$@"select
								id
							, item_name as {nameof(Weapon.Name)}
							, equipment_type as {nameof(Weapon.EquipmentType)}
							, category as {nameof(Weapon.Category)}
							, str as {nameof(Weapon.Str)}
							, agi as {nameof(Weapon.Agi)}
							, vit as {nameof(Weapon.Vit)}
							, wil as {nameof(Weapon.Wil)}
							, wis as {nameof(Weapon.Wis)}
							, strong_vs as {nameof(Weapon.StrongAgainst)}
							, magnetic as {nameof(Weapon.Magnetic)}
							, can_equip as {nameof(Weapon.CanEquip)}
							, icon as {nameof(Weapon.Icon)}
							, notes as {nameof(Weapon.Notes)}
							, def as {nameof(Armor.Defense)}
							, magic_def as {nameof(Armor.MagicDefense)}
							, evade as {nameof(Armor.Evade)}
							, magic_evade as {nameof(Armor.MagicEvade)}
							, status_protected as {nameof(Armor.StatusProtected)}
						from equipment.armor"
				);
			}
		}

		public async Task<Armor> GetArmorAsync(int armorId)
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryFirstOrDefaultAsync<Armor>
				(
					$@"select
								id
							, item_name as {nameof(Weapon.Name)}
							, equipment_type as {nameof(Weapon.EquipmentType)}
							, category as {nameof(Weapon.Category)}
							, str as {nameof(Weapon.Str)}
							, agi as {nameof(Weapon.Agi)}
							, vit as {nameof(Weapon.Vit)}
							, wil as {nameof(Weapon.Wil)}
							, wis as {nameof(Weapon.Wis)}
							, strong_vs as {nameof(Weapon.StrongAgainst)}
							, magnetic as {nameof(Weapon.Magnetic)}
							, can_equip as {nameof(Weapon.CanEquip)}
							, icon as {nameof(Weapon.Icon)}
							, notes as {nameof(Weapon.Notes)}
							, def as {nameof(Armor.Defense)}
							, magic_def as {nameof(Armor.MagicDefense)}
							, evade as {nameof(Armor.Evade)}
							, magic_evade as {nameof(Armor.MagicEvade)}
							, status_protected as {nameof(Armor.StatusProtected)}
						from equipment.armor
						where id = @id", armorId
				) ?? new Armor();
			}
		}

		public async Task<IEnumerable<Weapon>> GetWeaponsAsync()
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryAsync<Weapon>
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
							, icon as {nameof(Equipment.Icon)}
							, notes as {nameof(Equipment.Notes)}
							, attack as {nameof(Weapon.Attack)}
							, hit as {nameof(Weapon.Hit)}
							, status_inflicted as {nameof(Weapon.Casts)}
							, casts as {nameof(Weapon.StatusInflicted)}
							, throwable as {nameof(Weapon.Throwable)}
							, long_range as {nameof(Weapon.LongRange)}
							, two_handed as {nameof(Weapon.TwoHanded)}
						from equipment.weapons"
				);
			}
		}

		public async Task<Weapon> GetWeaponAsync(int weaponId)
		{
			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				return await connection.QueryFirstOrDefaultAsync<Weapon>
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
							, icon as {nameof(Equipment.Icon)}
							, notes as {nameof(Equipment.Notes)}
						from equipment.weapons
						where id = @id", weaponId
				) ?? new Weapon();
			}
		}
	}
}
