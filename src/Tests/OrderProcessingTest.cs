using core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
	public class OrderProcessingTest
	{
		[Test]
		public async Task Test_CreateOrder ()
		{
			IOptions<AppSettings> settings = ServiceProviderFactory.ServiceProvider.GetService<IOptions<AppSettings>>();
		}
	}
}
