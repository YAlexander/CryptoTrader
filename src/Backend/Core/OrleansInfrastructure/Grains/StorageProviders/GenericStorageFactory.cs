using System;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Storage;
using Persistence.Managers;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	internal class GenericStorageFactory
	{
		public static IGrainStorage Create<T, TK>(IServiceProvider services, string name) where T : IGrainStorage where TK : IDatabaseManager
		{
			// TODO: Replace managers with interfaces

			IOptions<DatabaseOptions> optionsSnapshot = services.GetRequiredService<IOptions<DatabaseOptions>>();
			DatabaseOptions options = optionsSnapshot.Value;
			TK storageManager = ActivatorUtilities.CreateInstance<TK>(services);
			object[] args = { options, storageManager };
			return ActivatorUtilities.CreateInstance<T>(services, args);
		}
	}
}
