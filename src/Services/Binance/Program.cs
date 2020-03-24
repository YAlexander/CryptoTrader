using Abstractions;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Entities;
using Persistence.Managers;
using Persistence.PostgreSQL.DbManagers;
using Persistence.PostgreSQL.Processors;

namespace Binance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddOptions();
                    services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
                    services.Configure<DatabaseOptions>(configuration.GetSection($"{nameof(AppSettings)}:{nameof(DatabaseOptions)}"));
                    
                    services.AddSingleton<OrleansClient>();
                    services.AddSingleton<IHostedService>(_ => _.GetService<OrleansClient>());
                    services.AddSingleton(_ => _.GetService<OrleansClient>().Client);

                    services.AddTransient<IExchangeSettingsManager, ExchangeSettingsManager>();
                    services.AddTransient<IOrderNotificator, OrderNotificator>();
                    services.AddTransient<ISettingsProcessor, ExchangeSettingsProcessor>();
                    services.AddTransient<ICandlesManager, CandlesManager>();
                    services.AddTransient<IExchangeOrderProcessor, BinanceOrderProcessor>();
                    services.AddTransient<ICandlesProcessor, CandlesProcessor>();
                    
                    services.AddHostedService<CandlesWorker>();
                    services.AddHostedService<TradesWorker>();
                    services.AddHostedService<TradingMonitor>();
                });
    }
}