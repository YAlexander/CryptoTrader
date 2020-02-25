using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using core;
using System.IO;

namespace Tests
{
	[SetUpFixture]
	public class Initializer
	{
		public static IHost _host;

		[OneTimeSetUp]
		public void OneTimeSetUp ()
		{
			InitServices();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown ()
		{

		}

		private static void InitServices ()
		{
			HostBuilder builder = new HostBuilder();
			_host = builder
			.ConfigureServices((hostContext, services) =>
			{
				IConfiguration config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.Build();

				services.AddOptions();
				services.Configure<AppSettings>(config.GetSection("AppSettings"));
				ServiceProviderFactory.ServiceProvider = services.BuildServiceProvider();
			})
			.Build();
		}
	}
}
