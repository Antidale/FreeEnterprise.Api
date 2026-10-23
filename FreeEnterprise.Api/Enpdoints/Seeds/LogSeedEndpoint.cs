using System;
using FeInfo.Common.DTOs;
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
        if (!TryValidateSeedInformation(logRequest.Info, out var validationResult))
        {
            return validationResult;
        }

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

    private static bool TryValidateSeedInformation(SeedInformation seedInformation, out IResult validationErrors)
    {
        IEnumerable<KeyValuePair<string, string[]>> errors = [];
        List<string> errorList = [];
        if (string.IsNullOrWhiteSpace(seedInformation.Flags))
        {
            errorList.Add($"{nameof(seedInformation.Flags)} is required and must have a value that is not null, empty, or solely whitespace");
        }

        if (string.IsNullOrWhiteSpace(seedInformation.Verification))
        {
            errorList.Add($"{nameof(seedInformation.Verification)} is required and must have a value that is not null, empty, or solely whitespace");
        }

        if (string.IsNullOrWhiteSpace(seedInformation.Version))
        {
            errorList.Add($"{nameof(seedInformation.Version)} is required and must have a value that is not null, empty, or solely whitespace");
        }

        if (!Uri.TryCreate(seedInformation.Url, UriKind.Absolute, out _))
        {
            errorList.Add($"{nameof(seedInformation.Url)} must be a valid absolute URL");
        }

        if (string.IsNullOrWhiteSpace(seedInformation.Seed))
        {
            errorList.Add($"{nameof(seedInformation.Seed)} is required and must have a value that is not null, empty, or solely whitespace");
        }

        if (errorList.Count != 0)
        {
            errors = errors.Append(new(nameof(LogSeedRoled.Info), [.. errorList]));
        }
        validationErrors = Results.ValidationProblem(errors);
        return !errors.Any();
    }
}
