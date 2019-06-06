
namespace FreeEnterprise.Api.Models
{
	public class Weapon : Equipment
	{
		public int Attack { get; set; }
		public int Hit { get; set; }
		public string Casts { get; set; }
		public string StatusInflicted { get; set; }
		public bool Throwable { get; set; }
		public bool LongRange { get; set; }
		public bool TwoHanded { get; set; }
	}
}
