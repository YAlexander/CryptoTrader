using System;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Storage;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	internal class GenericStorageFactory
	{
		public static IGrainStorage Create<T>(IServiceProvider services, string name) where T : IGrainStorage
		{
			IOptionsSnapshot<DatabaseOptions> optionsSnapshot = services.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
			DatabaseOptions options = optionsSnapshot.Get(name);;
			object[] args = { options };
			return ActivatorUtilities.CreateInstance<T>(services, args);
		}
	}
}
