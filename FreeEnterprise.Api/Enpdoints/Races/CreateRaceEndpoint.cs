using System;
using System.Reflection.Metadata.Ecma335;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Enpdoints.Races;

public static class CreateRaceEndpoint
{
    public static IEndpointRouteBuilder MapCreateRace(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Races.Create, async (
            [FromBody]
            CreateRaceRoom request,
            [FromServices]
            IRaceRepository repository
        ) =>
        {
            var insertResponse = await repository.CreateRaceAsync(request);
            if (insertResponse.Success)
                return Results.Ok(insertResponse.Data);

            return Results.Problem(detail: insertResponse.ErrorMessage, statusCode: (int)insertResponse.ErrorStatusCode!);
        })
        .WithTags(OpenApiMetadata.Races.ToString())
        .WithOrder((int)OpenApiMetadata.Races);

        return app;
    }


}
