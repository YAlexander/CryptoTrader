using System;
using System.Threading.Tasks;
using Abstractions.Grains;
using Common;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using Persistence.Entities;
using Persistence.Helpers;
using Persistence.PostgreSQL.Providers;
using TechanCore;

namespace Core.OrleansInfrastructure.Grains
{
	public class CandleGrain : Grain, ICandleGrain
	{
		private readonly IPersistentState<Candle> _candle;

		public CandleGrain(
			[PersistentState(nameof(Candle), nameof(CandleStorageProvider))] IPersistentState<Candle> candle)
		{
			_candle = candle;
		}

		public async Task Set(ICandle candle)
		{
			this.GetPrimaryKey(out string keyExt);

			if (_candle.State == null)
			{
				_candle.State = new Candle(candle, keyExt.ToExtendedKey());
				await _candle.WriteStateAsync();

				IStreamProvider streamProvider = GetStreamProvider(Constants.MessageStreamProvider);
				IAsyncStream<Candle> stream = streamProvider.GetStream<Candle>(Guid.NewGuid(), nameof(Candle));
				await stream.OnNextAsync(_candle.State);
			}
		}

		public Task<ICandle> Get()
		{
			return Task.FromResult((ICandle)_candle.State);
		}
	}
}