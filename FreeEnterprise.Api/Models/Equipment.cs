using System.Collections.Generic;

namespace FreeEnterprise.Api.Models
{
	public class Equipment
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string EquipmentType { get; set; }
		public string Category { get; set; }
		public IEnumerable<string> CanEquip { get; set; }
		public bool Magnetic { get; set; }
		public string StrongAgainst { get; set; }
		public int Str { get; set; }
		public int Agi { get; set; }
		public int Vit { get; set; }
		public int Wis { get; set; }
		public int Wil { get; set; }
		public string Icon { get; set; }
		public string Notes { get; set; }
	}
}
