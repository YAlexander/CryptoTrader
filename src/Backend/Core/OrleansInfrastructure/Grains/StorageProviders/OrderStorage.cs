﻿using System.Threading.Tasks;
using Abstractions.Enums;
using Common;
using Common.Trading;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;
using Persistence;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	public class OrderStorage : IGrainStorage
	{
		private readonly IOrdersProcessor _ordersProcessor;

		public OrderStorage(IOrdersProcessor ordersProcessor)
		{
			_ordersProcessor = ordersProcessor;
		}
		
		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			long primaryKey = grainReference.GetPrimaryKeyLong(out string keyExt);
			GrainKeyExtension keyExtension = keyExt.ToExtended();

			grainState.State = await _ordersProcessor.Get((Exchanges) primaryKey, keyExtension.Asset1, keyExtension.Asset2, keyExtension.Id);
		}

		public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			grainState.State = await _ordersProcessor.Update((IOrder)grainState.State);
		}

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			grainState.State = null;
			return Task.CompletedTask;
		}
	}
}