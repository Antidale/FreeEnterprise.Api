using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Providers;

namespace FreeEnterprise.Api.Repositories
{
	public class EquipmentRepository : IEquipmentRepository
	{
		private readonly IConnectionProvider _connectionProvider;

		public EquipmentRepository(ConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}
	}
}
