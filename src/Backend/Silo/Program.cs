using System.Net;
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
                .ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); })
                .UseOrleans((context, builder) =>
                {
                    builder.UseAdoNetClustering(options =>
                        {
                            options.Invariant = "Npgsql";
                            options.ConnectionString = "";
                        })
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "CryptoTrader";
                            options.ServiceId = "Backend";
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureLogging(logging => logging.AddNLog(new NLogProviderOptions
                        {
                            CaptureMessageTemplates = true,
                            CaptureMessageProperties = true
                        }))
                        //.ConfigureServices(services => { services.AddSingleton<IConfigurationRoot>(configuration); })
                        //.AddSimpleMessageStreamProvider(Constants.SMS_PROVIDER)
                        // .AddGenericGrainStorage<TraderStorageProvider>(nameof(TraderStorageProvider), opt =>
                        // {
                        // 	opt.Configure(options => { options.ConnectionString = ""; });
                        // })
                        .UseAdoNetReminderService(options =>
                        {
                            options.ConnectionString = "";
                            options.Invariant = "Npgsql";
                        })
                        .Configure<GrainVersioningOptions>(options =>
                        {
                            options.DefaultCompatibilityStrategy = nameof(BackwardCompatible);
                            options.DefaultVersionSelectorStrategy = nameof(MinimumVersion);
                        })
                        .UseDashboard(options =>
                        {
                            //options.Username = "USERNAME";
                            //options.Password = "PASSWORD";
                            options.Host = "*";
                            options.Port = 3128;
                            options.HostSelf = true;
                            options.CounterUpdateIntervalMs = 5000;
                        });
                });
    }
}