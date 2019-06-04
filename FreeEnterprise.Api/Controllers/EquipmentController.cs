using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
