using System.Collections.Generic;
using System.Threading.Tasks;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Requests;

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
