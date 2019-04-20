using FreeEnterprise.Api.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FreeEnterprise.Api.Providers
{
	public class ConnectionProvider : IConnectionProvider
	{
		private readonly IConfiguration _configuration;

		public ConnectionProvider(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IDbConnection GetConnection()
		{
			var connectionString = _connectionString;

			return new NpgsqlConnection(_connectionString);
		}

		private string _connectionString => $"HOST={_getConfigValue("PGHOST", "localhost")};Port={_getConfigValue("PGPORT")};Database={_getConfigValue("PGDATABASE")};User Id={_getConfigValue("PGUSER")};Password={_getConfigValue("PGPASS")};";

		private string _getConfigValue(string name, string defaultValue = "") => _configuration.GetValue(name, defaultValue);
	}
}
