using System.Data;

namespace FreeEnterprise.Api.Interfaces
{
	public interface IConnectionProvider
	{
		IDbConnection GetConnection();
	}
}
