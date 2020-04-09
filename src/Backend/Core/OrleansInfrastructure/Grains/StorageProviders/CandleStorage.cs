using System.Threading.Tasks;
using Abstractions.Enums;
using Common;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;
using Persistence;
using Persistence.Entities;
using Persistence.Helpers;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	public class CandleStorage : IGrainStorage
	{
		private readonly ICandlesProcessor _candlesProcessor;
		
		public CandleStorage(ICandlesProcessor candlesProcessor)
		{
			_candlesProcessor = candlesProcessor;
		}

		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			long primaryKey = grainReference.GetPrimaryKeyLong(out string keyExt);
			GrainKeyExtension keyExtension = keyExt.ToExtendedKey();

			if (keyExtension.Time.HasValue)
			{
				grainState.State = await _candlesProcessor.GetCandle((Exchanges) primaryKey, keyExtension.Asset1, keyExtension.Asset2, keyExtension.Time.Value);
			}
		}

		public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			await _candlesProcessor.Create((Candle) grainState.State);
		}

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			grainState.State = null;
			return Task.CompletedTask;
		}
	}
}