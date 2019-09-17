using System.IO;
using core;
using core.Abstractions.Database;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Managers;
using core.Infrastructure.Notifications.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingProcessor.Workers;

namespace TradingProcessor
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
				.ConfigureServices((hostContext, services) =>
				{
					IConfigurationRoot configuration = new ConfigurationBuilder()
														.SetBasePath(Directory.GetCurrentDirectory())
														.AddJsonFile("appsettings.json", false, true)
														.AddEnvironmentVariables()
														.Build();

					services.AddOptions();
					services.AddMemoryCache();

					services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
					services.AddTransient<IPairConfigManager, PairConfigManager>();
					services.AddTransient<IExchangeConfigManager, ExchangeConfigManager>();
					services.AddTransient<ICandleManager, CandleManager>();
					services.AddTransient<IStrategyManager, StrategyManager>();
					services.AddSingleton<ExchangeConfigProcessor>();
					services.AddSingleton<CandleProcessor>();
					services.AddTransient<TelegramClient>();

					services.AddSingleton<TradingContextBuilder>();

					services.AddHostedService<ForecastProcessor>();
				});
	}
}
