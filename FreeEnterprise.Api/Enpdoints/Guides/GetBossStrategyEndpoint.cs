using System.Text.Json.Serialization;
using FeInfo.Common.Enums;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Guides;

public static class GetBossStrategyEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetBossStrategy()
        {
            app.MapGet(ApiEndpoints.Guides.BossStrat, async (
                [FromServices]
                IGuidesRepository repository,
                [FromRoute]
                BossName bossName
            ) =>
            {
                var result = await repository.GetBossStrategyAsync(bossName);
                return result.ToResult();
            })
            .WithTags(OpenApiMetadata.Guide.ToString())
            .WithOrder((int)OpenApiMetadata.Guide);

            return app;
        }
    }
}
