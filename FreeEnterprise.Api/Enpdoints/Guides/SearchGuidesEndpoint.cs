using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Guides;

public static class SearchGuidesEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGuideSearch()
        {
            app.MapGet(ApiEndpoints.Guides.Search, SearchAsync)
                .WithTags(OpenApiMetadata.Guide.ToString())
                .WithOrder((int)OpenApiMetadata.Guide);
            return app;
        }
    }

    private static async Task<IResult> SearchAsync(
        [FromServices]
        IGuidesRepository repository,
        [FromQuery]
        string searchText,
        [FromQuery]
        int? limit = null
    )
    {
        var result = await repository.GetGuidesAsync(searchText, limit);
        return result.ToResult();
    }
}
