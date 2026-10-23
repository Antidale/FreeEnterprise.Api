using System;
using System.ComponentModel;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Seeds;

public static class GetSeedsEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetSeedsEndpoint()
        {
            app.MapGet(ApiEndpoints.Seeds.SearchSeeds, GetSeedsAsync)
                .WithTags(OpenApiMetadata.Seeds.ToString())
                .WithOrder((int)OpenApiMetadata.Seeds);
            return app;
        }
    }

    public static async Task<IResult> GetSeedsAsync
    (
        [FromServices]
        ISeedRepository repository,
        [Description("Skips this amount of records in your data set. Combined with limit, used with to page through results.")]
        [FromQuery] int offset = 0,
        [Description("The maximum amount of SeedDetail objects to return from your query. Currently no limit is set, but this can change without warning in the future. Combine with offset to page through results")]
        [FromQuery] int limit = 20,
        [Description("Search for a flagset that contains a desired flag. Can be a bit wonky.")]
        [FromQuery] string? flagSearch = null,
        [Description("search for an exact match of the seed value.")]
        [FromQuery] string? seedString = null,
        [Description("search for an exact match of the binary flag string.")]
        [FromQuery] string? binaryFlags = null,
        [Description("when true, the seed must have a copy of the patch page saved")]
        [FromQuery] bool? onlySavedHtml = false
    )
    {
        if (flagSearch == string.Empty)
        {
            flagSearch = null;
        }

        if (seedString == string.Empty)
        {
            seedString = null;
        }

        if (binaryFlags == string.Empty)
        {
            binaryFlags = null;
        }

        var response = await repository.SearchSeedDetails(offset: offset, limit: limit, flagset: flagSearch, seedValue: seedString, binaryFlags: binaryFlags, onlySavedHtml: onlySavedHtml ?? false);

        return response.ToResult();
    }
}
