namespace FreeEnterprise.Api.Models
{
	public class BossStat
	{
		public int Id { get; set; }
		public string Location { get; set; }
		public string Battle { get; set; }
		public string Enemy { get; set; }
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
		public string ScriptValue1 { get; set; }
		public string ScriptValue2 { get; set; }
		public string ScriptValue3 { get; set; }
		public string ScriptValue4 { get; set; }
	}
}
