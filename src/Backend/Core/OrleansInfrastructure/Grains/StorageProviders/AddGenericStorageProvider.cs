﻿using System;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Storage;
using Persistence.Managers;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	public static class AddGenericStorageProvider
	{
		public static ISiloBuilder AddGenericGrainStorage<T, TK>(this ISiloBuilder builder, string name, Action<OptionsBuilder<DatabaseOptions>> options = null) where T : IGrainStorage where TK : IDatabaseManager
		{
			return builder.ConfigureServices(services =>
			{
				options?.Invoke(services.AddOptions<DatabaseOptions>(name));
				services.ConfigureNamedOptionForLogging<DatabaseOptions>(name);
				services.TryAddSingleton<IGrainStorage>(sp => sp.GetServiceByName<IGrainStorage>(nameof(T)));
				services.AddSingletonNamedService<IGrainStorage>(name, GenericStorageFactory.Create<T, TK>);
			});
		}
	}
}