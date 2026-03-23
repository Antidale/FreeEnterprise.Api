using System.Reflection;
using Dapper;
using FreeEnterprise.Api;
using FreeEnterprise.Api.Constraints;
using FreeEnterprise.Api.Extensions;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Providers;
using FreeEnterprise.Api.Repositories;
using FreeEnterprise.Api.Services;
using FreeEnterprise.Api.TypeHandlers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
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

// Add services to the container.
builder.Services.AddSingleton<ISeedFetchService, SeedFetchSerivce>()
                .AddSingleton<IConnectionProvider, ConnectionProvider>()
                .AddRepositories()
                .AddHttpClient()
                .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.SchemaFilter<EnumSchemaFilter>();
});
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<RacetimeDataFetchService>();

SqlMapper.AddTypeHandler(new StringListHandler());
SqlMapper.AddTypeHandler(new JsonStringDictionaryHandler());
var app = builder.Build();

app.UseSwagger();
app.MapOpenApi().CacheOutput();
app.MapScalarApiReference(options => options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json"));

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.Run();
