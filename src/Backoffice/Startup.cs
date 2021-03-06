using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.Abstractions;
using core.Abstractions.Database;
using core.Infrastructure;
using core.Infrastructure.BL;
using core.Infrastructure.BL.OrderProcessors;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using core.Infrastructure.OrdersProcessing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backoffice
{
	public class Startup
	{
		public Startup (IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services)
		{
			services.AddOptions();
			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

			services.AddTransient<IPairConfigManager, PairConfigManager>();
			services.AddTransient<IStrategyManager, StrategyManager>();
			services.AddTransient<IAssetManager, AssetManager>();
			services.AddTransient<IOrderManager, OrdersManager>();
			services.AddTransient<IExchangeOrdersSender, BinanceOrderSender>();
			services.AddTransient<IExchangeConfigManager, ExchangeConfigManager>();

			services.AddTransient<ExchangeConfigProcessor>();
			services.AddTransient<StrategyProcessor>();
			services.AddTransient<AssetProcessor>();
			services.AddTransient<PairConfigProcessor>();
			services.AddTransient<OrderProcessor>();
			services.AddTransient<NatsConnector>();

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
