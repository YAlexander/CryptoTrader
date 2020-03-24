using System.Net;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Versions.Compatibility;
using Orleans.Versions.Selector;
using Persistence.PostgreSQL;

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
                    DatabaseOptions dbOptions = new DatabaseOptions();
                    hostContext.Configuration.GetSection("AppSettings:DatabaseOptions").Bind(dbOptions);

                    if (dbOptions.MigrateDatabaseOnStart)
                    {
                        EvolveMigrator migrator = new EvolveMigrator();
                        migrator.Migrate(dbOptions.SystemConnectionString, "Migrations/System", false);
                    }

                    services.AddHostedService<Worker>();
                })
                .UseOrleans((context, builder) =>
                {
                    AppSettings appSettings = new AppSettings();
                    context.Configuration.GetSection("AppSettings:DatabaseOptions").Bind(appSettings);
                    
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
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Any)
                        .ConfigureLogging(logging => logging.AddNLog(new NLogProviderOptions
                        {
                            CaptureMessageTemplates = true,
                            CaptureMessageProperties = true
                        }))
                        //.ConfigureServices(services => { services.AddSingleton<IConfigurationRoot>(configuration); })
                        .AddSimpleMessageStreamProvider(Constants.MessageStreamProvider)
                        // .AddGenericGrainStorage<TraderStorageProvider>(nameof(TraderStorageProvider), opt =>
                        // {
                        // 	opt.Configure(options => { options.ConnectionString = ""; });
                        // })
                        .UseAdoNetReminderService(options =>
                        {
                            options.ConnectionString = "";
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