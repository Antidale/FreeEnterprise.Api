using System.Reflection;
using Dapper;
using FreeEnterprise.Api;
using FreeEnterprise.Api.Constraints;
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
builder.Services.AddSingleton<IBattleLocationsRepository, BattleLocationsRepository>();
builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>();
builder.Services.AddSingleton<IBossBattlesRepository, BossBattlesRepository>();
builder.Services.AddSingleton<IBossStatsRepository, BossStatsRepository>();
builder.Services.AddSingleton<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddSingleton<ITournamentRepository, TournamentRepository>();
builder.Services.AddSingleton<IEntrantRepository, EntrantRepository>();
builder.Services.AddSingleton<IGuidesRepository, GuidesRepository>();
builder.Services.AddSingleton<ISeedRepository, SeedRepository>();
builder.Services.AddSingleton<ISeedFetchService, SeedFetchSerivce>();
builder.Services.AddSingleton<IRaceRespository, RaceRepository>();
builder.Services.AddSingleton<IRacerRepository, RacerRepository>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

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

SqlMapper.AddTypeHandler(new StringListHandler());
SqlMapper.AddTypeHandler(new JsonStringDictionaryHandler());
var app = builder.Build();

app.UseSwagger();
app.MapOpenApi().CacheOutput();
app.MapScalarApiReference(options => options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json"));

Console.WriteLine("Application Starting");
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.Run();
