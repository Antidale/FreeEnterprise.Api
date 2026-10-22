using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace FreeEnterprise.Api;

public class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context is null || context.JsonPropertyInfo is null)
        {
            return Task.CompletedTask;
        }

        if (!context.JsonPropertyInfo.PropertyType.IsEnum)
        {
            return Task.CompletedTask;
        }

        if (schema is null || schema.Enum is null)
        {
            return Task.CompletedTask;
        }

        schema.Enum.Clear();

        foreach (var name in Enum.GetNames(context.JsonPropertyInfo.PropertyType))
        {
            schema.Enum.Add(name);
        }

        return Task.CompletedTask;
    }
}
