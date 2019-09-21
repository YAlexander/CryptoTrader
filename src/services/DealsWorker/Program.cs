using System;
using System.IO;
using core;
using core.Abstractions.Database;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using TradesWorker.Workers;
using Microsoft.Extensions.Logging;

namespace TradesWorker
{
	public class Program
	{
		public static void Main (string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder (string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.AddConsole();
					logging.SetMinimumLevel(LogLevel.Trace);
				})
				.UseSystemd()
				.ConfigureServices((hostContext, services) =>
				{
					IConfigurationRoot configuration = new ConfigurationBuilder()
									.SetBasePath(Directory.GetCurrentDirectory())
									.AddJsonFile("appsettings.json", false, true)
									.AddEnvironmentVariables()
									.Build();

					services.AddOptions();
					services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

					services.AddTransient<IPairConfigManager, PairConfigManager>();
					services.AddTransient<IExchangeConfigManager, ExchangeConfigManager>();
					services.AddTransient<ITradesManager, TradesManager>();

					services.AddSingleton<ExchangeConfigProcessor>();
					services.AddSingleton<TradesProcessor>();

					services.AddHostedService<BinanceWorker>();
				})
				.UseNLog();
	}
}
