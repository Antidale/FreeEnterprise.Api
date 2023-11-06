using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Providers;
using FreeEnterprise.Api.Repositories;

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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.Run();
