namespace FreeEnterprise.Api.Models
{
	public class Equipment
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string EquipmentType { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public IEnumerable<string> CanEquip { get; set; } = Enumerable.Empty<string>();
		public bool Magnetic { get; set; }
		public string StrongAgainst { get; set; } = string.Empty;
		public int Str { get; set; }
		public int Agi { get; set; }
		public int Vit { get; set; }
		public int Wis { get; set; }
		public int Wil { get; set; }
		public string Icon { get; set; } = string.Empty;
		public string Notes { get; set; } = string.Empty;
	}
}
