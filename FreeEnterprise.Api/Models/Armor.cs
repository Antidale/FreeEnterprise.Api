
namespace FreeEnterprise.Api.Models
{
	public class Armor : Equipment
	{
		public int Defense { get; set; }
		public int MagicDefense { get; set; }
		public int Evade { get; set; }
		public int MagicEvade { get; set; }
		public IEnumerable<string> StatusProtected { get; set; } = Enumerable.Empty<string>();
	}
}
