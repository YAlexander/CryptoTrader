using System.Net;
using Common;
using Core.OrleansInfrastructure.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Versions.Compatibility;
using Orleans.Versions.Selector;

namespace Silo
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
                    services.AddOptions();
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
                    services.Configure<DatabaseOptions>(hostContext.Configuration.GetSection(nameof(DatabaseOptions)));
                    
                    services.AddHostedService<Worker>();
                })
                .UseOrleans((context, builder) =>
                {
                    AppSettings appSettings = context.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
                    
                    builder.UseAdoNetClustering(options =>
                        {
                            options.Invariant = appSettings.ClusterInvariant;
                            options.ConnectionString = appSettings.DatabaseOptions.SystemConnectionString;
                        })
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = appSettings.ClusterId;
                            options.ServiceId = appSettings.ServiceId;
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureLogging(logging => logging.AddNLog(new NLogProviderOptions
                        {
                            CaptureMessageTemplates = true,
                            CaptureMessageProperties = true
                        }))
                        .AddSimpleMessageStreamProvider(Constants.MessageStreamProvider)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TradingGrain).Assembly).WithReferences())
                        //.AddMemoryGrainStorage(name: "PubSubStore")
                        .AddAdoNetGrainStorage(Constants.PubSubStorage, optionsBuilder =>
                        {
                            optionsBuilder.ConnectionString = appSettings.DatabaseOptions.SystemConnectionString;
                        	optionsBuilder.Invariant = appSettings.ClusterInvariant;
                         	optionsBuilder.UseJsonFormat = true;
                        })
                        // .AddGenericGrainStorage<TraderStorageProvider>(nameof(TraderStorageProvider), opt =>
                        // {
                        // 	opt.Configure(options => { options.ConnectionString = ""; });
                        // })
                        .UseAdoNetReminderService(options =>
                        {
                            options.ConnectionString = appSettings.DatabaseOptions.SystemConnectionString;
                            options.Invariant = appSettings.ClusterInvariant;
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