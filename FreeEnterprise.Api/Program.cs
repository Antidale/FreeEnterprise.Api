﻿using Dapper;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Providers;
using FreeEnterprise.Api.Repositories;
using FreeEnterprise.Api.Services;
using FreeEnterprise.Api.TypeHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Configuration.AddEnvironmentVariables(prefix: "FE_");

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
builder.Services.AddHttpClient();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

SqlMapper.AddTypeHandler(new StringListHandler());
SqlMapper.AddTypeHandler(new JsonStringDictionaryHandler());
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

Console.WriteLine("Application Starting");
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.Run();
