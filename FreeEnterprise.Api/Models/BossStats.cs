namespace FreeEnterprise.Api.Models
{
	public class BossStat
	{
		public int Id { get; set; }
		public string Location { get; set; } = string.Empty;
		public string Battle { get; set; } = string.Empty;
		public string Enemy { get; set; } = string.Empty;
		public int Level { get; set; }
		public int HitPoints { get; set; }
		public int ExperiencePoints { get; set; }
		public int Gil { get; set; }
		public int AttackMultiplier { get; set; }
		public int AttackPercent { get; set; }
		public int AttackPower { get; set; }
		public int DefenseMultiplier { get; set; }
		public int Evade { get; set; }
		public int Defense { get; set; }
		public int MagicDefenseMultiplier { get; set; }
		public int MagicDefense { get; set; }
		public int MagicEvade { get; set; }
		public int MinSpeed { get; set; }
		public int MaxSpeed { get; set; }
		public int SpellPower { get; set; }
		public string Notes { get; set; } = string.Empty;
	}
}
