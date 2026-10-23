using System;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Seeds;

public static class LogSeedEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapLogSeedEndpoint()
        {
            app.MapPost(ApiEndpoints.Seeds.SearchSeeds, LogSeedAsync)
                .WithTags(OpenApiMetadata.Seeds.ToString())
                .WithOrder((int)OpenApiMetadata.Seeds)
                .WithDescription("Saves the patch page to the database for a given race room. Requires a valid API key to use.")
                .RequireAuthorization()
                .ProducesProblem(StatusCodes.Status403Forbidden)
                .ProducesProblem(StatusCodes.Status401Unauthorized);
            return app;
        }
    }

    public static async Task<IResult> LogSeedAsync
    (
        [FromServices]
        ISeedRepository repository,
        [FromServices]
        ISeedFetchService seedFetchService,
        [FromBody]
        LogSeedRoled logRequest
    )
    {
        var insertResponse = await repository.SaveSeedRolledAsync(logRequest);
        if (!insertResponse.Success)
        {
            return insertResponse.ToResult();
        }

        var patchHtmlResponse = await seedFetchService.GetPatchHtmlAsync(logRequest.Info);
        if (patchHtmlResponse.Success && patchHtmlResponse.Data is not null)
        {
            await repository.SavePatchHtml(insertResponse.Data, patchHtmlResponse.Data);
        }

        //Intentionally swallowing any errors with saving the patch page
        return insertResponse.ToResult();
    }
}
