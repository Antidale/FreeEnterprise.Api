using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Repositories;
using FreeEnterprise.Api.Providers;
using Swashbuckle.AspNetCore.Swagger;

namespace FreeEnterprise.Api.BossStats
{
	public class Startup
	{
		readonly string _allowedOrigins = "_allowedOrigins";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(opt =>
			{
				opt.AddPolicy(_allowedOrigins, builder =>
				{
					builder.WithOrigins("http://localhost:3000")
						.AllowAnyHeader();
				});
			});

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddSwaggerGen(c =>
					{
						c.SwaggerDoc("v1", new Info { Title = "Free Enterprise Info API", Version = "v1" });
					});
			services.AddSingleton<IBattleLocationsRepository, BattleLocationsRepository>();
			services.AddSingleton<IConnectionProvider, ConnectionProvider>();
			services.AddSingleton<IBossBattlesRepository, BossBattlesRepository>();
			services.AddSingleton<IBossStatsRepository, BossStatsRepository>();
			services.AddSingleton<IEquipmentRepository, EquipmentRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors(_allowedOrigins);
			app.UseHttpsRedirection();
			app.UseSwagger();
			app.UseSwaggerUI(x =>
			{
				x.SwaggerEndpoint("/swagger/v1/swagger.json", "Free Enterprise Info API");
			});
			app.UseMvc();

		}
	}
}
