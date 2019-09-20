using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using OrdersProcessor.Workers;
using Microsoft.Extensions.Logging;

namespace OrdersProcessor
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
					services.AddHostedService<OrdersProcessingWorker>();
				})
				.UseNLog();
	}
}
