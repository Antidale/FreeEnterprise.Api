using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IEquipmentRepository
	{
		Task<IEnumerable<Equipment>> GetEquipmentAsync();
		Task<IEnumerable<Armor>> GetArmorAsync();
		Task<Armor> GetArmorAsync(int armorId);
		Task<IEnumerable<Weapon>> GetWeaponsAsync();
		Task<Weapon> GetWeaponAsync(int weaponId);
	}
}
