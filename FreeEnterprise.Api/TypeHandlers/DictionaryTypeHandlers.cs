using System.Data;
using System.Text.Json;
using Dapper;
using Npgsql;
using NpgsqlTypes;


namespace FreeEnterprise.Api.TypeHandlers;

public class JsonStringDictionaryHandler : SqlMapper.TypeHandler<Dictionary<string, string>>
{
    public override Dictionary<string, string> Parse(object value)
    {
        if (value is null || value.ToString() is null) return [];

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(value.ToString()!) ?? [];
        }
        catch (Exception)
        {
            return [];
        }
    }

    public override void SetValue(IDbDataParameter parameter, Dictionary<string, string>? value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
        if (parameter is NpgsqlParameter npgsqlParameter)
        {
            npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
        }
    }

}
