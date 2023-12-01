using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class EquipmentController(IEquipmentRepository equipmentRepository) : ControllerBase
	{
		private readonly IEquipmentRepository _equipmentRepository = equipmentRepository;

        [HttpGet]
		public async Task<ActionResult<IEnumerable<Equipment>>> Get()
		{
			var equipment = await _equipmentRepository.GetEquipmentAsync();
			return Ok(equipment);
		}

		[HttpGet("Armor")]
		public async Task<ActionResult<IEnumerable<Armor>>> GetArmor()
		{
			var armorList = await _equipmentRepository.GetArmorAsync();
			return Ok(armorList);
		}

		[HttpGet("Armor/{id:int}")]
		public async Task<ActionResult<Armor>> GetArmor(int id)
		{
			var armor = await _equipmentRepository.GetArmorAsync(id);
			return armor == null ? NotFound() : Ok(armor);
		}

		[HttpGet("Weapons")]
		public async Task<ActionResult<IEnumerable<Weapon>>> GetWeapons()
		{
			var weapons = await _equipmentRepository.GetWeaponsAsync();
			return Ok(weapons);
		}

		[HttpGet("Weapons/{id:int}")]
		public async Task<ActionResult<Weapon>> GetWeapon(int id)
		{
			var weapon = await _equipmentRepository.GetWeaponAsync(id);
			return weapon == null ? NotFound() : Ok(weapon);
		}
	}
}
