using System.ComponentModel;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Enpdoints.Test;

public static class GetTestsEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetTests()
        {
            app.MapGet(ApiEndpoints.Test.GetAll, GetTestsAsync)
            .Produces<TestResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            //In Scalar this drives the left column
            .WithTags(OpenApiMetadata.Testing.ToString())
            .WithOrder((int)OpenApiMetadata.Testing)
            .WithDescription("Description, memory cache 5 minutes");

            return app;
        }
    }

    private static async Task<IResult> GetTestsAsync(
        [FromServices]
        IMemoryCache memoryCache,
        [Description("The amount of records to skip. Combined with limit, lets you page through results")]
        int offset = 0,
        [Description("The amount of records to return. Max 500.")]
        int limit = 20,
        [Description("By default, races with a status of 'cancelled' are excluded. set to true to also return these")]
        bool includeCancelled = false,
        [Description("A word or phrase used in the race's description. If you're looking for races for a specific racer, use the /racers endoints instead.")]
        string? description = null,
        [Description("A portion of the flagset you want to search for. There are some oddities to this.")]
        string? flagset = null)
    {
        if (limit > 500)
        {
            return Results.Problem("Cannot ask for more than 500 races", statusCode: StatusCodes.Status400BadRequest);
        }

        var cacheKey = $"Tests_o{offset}_l{limit}_d{description}_f{flagset}_{includeCancelled}";

        if (memoryCache.TryGetValue<TestResponse>(cacheKey, out var cached) && cached is not null)
        {
            var updatedCache = cached with { FromCache = true };
            return Response.SetSuccess(updatedCache).ToResult();
        }

        var time = DateTime.UtcNow;
        var data = new TestResponse(time, limit, offset, includeCancelled, description, flagset, false);

        memoryCache.SetCache(cacheKey, data);
        return Response.SetSuccess(data).ToResult();
    }
}

public record TestResponse(DateTime Timestamp, int Limit, int Offset, bool IncludeCancelled, string? Description, string? Flagset, bool FromCache);
