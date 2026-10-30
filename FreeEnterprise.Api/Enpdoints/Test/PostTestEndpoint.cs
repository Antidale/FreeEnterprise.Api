using System;
using System.Text.Json.Serialization;

namespace FreeEnterprise.Api.Enpdoints.Test;

public static class PostTestEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapPostTestEndpoint()
        {
            app.MapPost(ApiEndpoints.Test.Create, async
            (
                [FromBody]
                TestPost post
            ) =>
            {
                return Results.Created();
            })
            .WithTags(OpenApiMetadata.Testing.ToString())
            .WithOrder((int)OpenApiMetadata.Testing);
            return app;
        }
    }
}

public class TestPost
{
    public required string Title { get; set; }
    public required TestEnum Status { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<TestEnum>))]
public enum TestEnum
{
    First,
    Second,
    Third
}
