using System;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Requests;

namespace FreeEnterprise.Api.Enpdoints.BossInfo;

public static class GetBossStatsEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetBossStats()
        {
            app.MapGet(ApiEndpoints.BossStats.GetAll, GetBossStatsAsync)
               .WithTags("BossInfo")
               .Produces(StatusCodes.Status400BadRequest)
               .Produces<IEnumerable<BossStat>>();
            return app;
        }
    }

    public static async Task<IResult> GetBossStatsAsync(
        [FromServices]
        IBossStatsRepository repository,
        [FromBody]
        BossStatsSearchRequest request
    )
    {
        if (request.BattleId == 0 && request.LocationId == 0)
            return Results.Problem(
                $"both {nameof(request.BattleId)} and {nameof(request.LocationId)} cannot be zero",
                statusCode: StatusCodes.Status400BadRequest);

        var result = repository.SearchAsync(request);
        return TypedResults.Ok(result);
    }
}
