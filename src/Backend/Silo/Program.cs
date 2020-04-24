using System;
using System.Net;
using Common;
using Core.OrleansInfrastructure.Grains;
using Core.OrleansInfrastructure.Grains.StorageProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Versions.Compatibility;
using Orleans.Versions.Selector;
using Persistence.PostgreSQL.DbManagers;
using Persistence.PostgreSQL.Providers;

namespace Silo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Starting Silo");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped Silo because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
                    services.Configure<DatabaseOptions>(hostContext.Configuration.GetSection($"{nameof(AppSettings)}:{nameof(DatabaseOptions)}"));
                    
                    services.AddSingleton<OrleansClient>();

                    services.AddHostedService<CandlesMonitor>();
                })
                .UseOrleans((context, builder) =>
                {
                    AppSettings appSettings = context.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
                    
                    builder
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = appSettings.ClusterId;
                            options.ServiceId = appSettings.ServiceId;
                        })
                        .UseAdoNetClustering(options =>
                        {
                             options.Invariant = appSettings.DatabaseOptions.DatabaseProvider;
                             options.ConnectionString = appSettings.DatabaseOptions.SystemConnectionString;
                        })
                        .Configure<EndpointOptions>(options =>
                        {
                            IPAddress.TryParse(appSettings.AdvertisedIPAddress, out var ip);
                            options.AdvertisedIPAddress = ip;

                            //// Port to use for Silo-to-Silo
                            //options.SiloPort = 11111;
                            //// Port to use for the gateway
                            //options.GatewayPort = 30000;
                            //// IP Address to advertise in the cluster
                            //options.AdvertisedIPAddress = IPAddress.Parse("172.16.0.42");
                            //// The socket used for silo-to-silo will bind to this endpoint
                            //options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 40000);
                            //// The socket used by the gateway will bind to this endpoint
                            //options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 50000);
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.AddNLog(new NLogProviderOptions
                            {
                                IgnoreEmptyEventId = true,
                                CaptureMessageTemplates = true,
                                CaptureMessageProperties = true
                            });
                        })
                        .AddSimpleMessageStreamProvider(Constants.MessageStreamProvider)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TradingGrain).Assembly).WithReferences())
                        .AddAdoNetGrainStorage(Constants.PubSubStorage, optionsBuilder =>
                        {
                            optionsBuilder.ConnectionString = appSettings.DatabaseOptions.SystemConnectionString;
                        	optionsBuilder.Invariant = appSettings.DatabaseOptions.DatabaseProvider;
                         	optionsBuilder.UseJsonFormat = true;
                        })
                        .AddGenericGrainStorage<CandleStorageProvider, CandlesManager>(nameof(CandleStorageProvider), opt =>
                        {
                            opt.Configure(options =>
                            {
                                options.CryptoTradingConnectionString = appSettings.DatabaseOptions.CryptoTradingConnectionString;
                            });
                        })
                        .AddGenericGrainStorage<OrdersStorageProvider, OrdersManager>(nameof(OrdersStorageProvider), opt =>
                        {
                            opt.Configure(options =>
                                {
                                    options.CryptoTradingConnectionString = appSettings.DatabaseOptions.CryptoTradingConnectionString;
                                });
                        })
                        .AddGenericGrainStorage<DealsStorageProvider, DealsManager>(nameof(DealsStorageProvider), opt => 
                        {
                            opt.Configure(options =>
                                {
                                    options.CryptoTradingConnectionString = appSettings.DatabaseOptions.CryptoTradingConnectionString;
                                });
                        })
                        .UseAdoNetReminderService(options =>
                        {
                            options.ConnectionString = appSettings.DatabaseOptions.SystemConnectionString;
                            options.Invariant = appSettings.DatabaseOptions.DatabaseProvider;
                        })
                        .Configure<GrainVersioningOptions>(options =>
                        {
                            options.DefaultCompatibilityStrategy = nameof(BackwardCompatible);
                            options.DefaultVersionSelectorStrategy = nameof(MinimumVersion);
                        })
                        .UseDashboard(options =>
                        {
                            options.Username = appSettings.DashboardUser;
                            options.Password = appSettings.DashboardPassword;
                            options.Host = "*";
                            options.Port = 3128;
                            options.HostSelf = true;
                            options.CounterUpdateIntervalMs = 5000;
                        });
                });
    }
}