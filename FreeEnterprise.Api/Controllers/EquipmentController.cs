using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EquipmentController : ControllerBase
	{
		private readonly IEquipmentRepository _equipmentRepository;

		public EquipmentController(IEquipmentRepository equipmentRepository)
		{
			_equipmentRepository = equipmentRepository;
		}

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
			return Ok(armor);
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
			return Ok(weapon);
		}
	}
}
