using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FreeEnterprise.Api;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        if (schema is null || schema.Enum is null)
        {
            return;
        }

        schema.Enum.Clear();

        foreach (var name in Enum.GetNames(context.Type))
        {
            schema.Enum.Add(name);
        }
    }

}
