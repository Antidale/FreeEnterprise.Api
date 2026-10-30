using System.Text.Json.Serialization;
using Dapper;
using FreeEnterprise.Api.Auth;
using FreeEnterprise.Api.Constraints;
using FreeEnterprise.Api.Enpdoints;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Providers;
using FreeEnterprise.Api.Services;
using FreeEnterprise.Api.TypeHandlers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.ConfigureHttpJsonOptions(opt =>
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Configuration.AddEnvironmentVariables(prefix: "FE_");

//I actually think this is better handled a different way, because getting a BadRequest back for invalid values is much more appropriate than a 404, but I thought it was interesting to do the work for this and try it out
builder.Services.Configure<RouteOptions>(opt =>
{
    opt.ConstraintMap.Add("bossNameEnum", typeof(BossNameRouteConstraint));
});

builder.Services.AddAuthentication(ApiKeyAuthOptions.DefaultScheme)
                .AddScheme<ApiKeyAuthOptions, ApiKeyAuthenticationHandler>
                (
                    ApiKeyAuthOptions.DefaultScheme, _ => { }
                );

//TODO: renable if/when I actually add any sort of authorization.
//builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();

// Add services to the container.
builder.Services.AddSingleton<ISeedFetchService, SeedFetchSerivce>()
                .AddSingleton<IConnectionProvider, ConnectionProvider>()
                .AddSingleton<IRacetimeDataService, RacetimeDataService>()
                .AddRepositories()
                .AddHttpClient()
                //TODO: Track down what is still using this at all, this should not be needed anymore
                .AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.AddHostedService<RacetimeRaceUpdateService>();

SqlMapper.AddTypeHandler(new StringListHandler());
SqlMapper.AddTypeHandler(new JsonStringDictionaryHandler());
var app = builder.Build();

app.MapOpenApi().CacheOutput();
app.MapScalarApiReference(opt =>
{
    opt.Title = "FeInfo API";
    opt.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    opt.WithFeaturedClients
    (
        new(ScalarTarget.Node, ScalarClient.Fetch),
        new(ScalarTarget.Shell, ScalarClient.Curl),
        new(ScalarTarget.Python, ScalarClient.Requests)
    );
    opt.DisableAgent();
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();
app.MapEndpoints();

app.Run();
