using System;
using System.IO;
using CandlesWorker.Workers;
using core;
using core.Abstractions.Database;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CandlesWorker
{
	public class Program
	{
		public static void Main (string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder (string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSystemd()
				.ConfigureServices(services =>
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
					services.AddTransient<ICandleManager, CandleManager>();

					services.AddSingleton<ExchangeConfigProcessor>();
					services.AddSingleton<CandleProcessor>();

					services.AddHostedService<BinanceWorker>();
					services.AddHostedService<BinanceCandlesMonitor>();
				});
	}
}
