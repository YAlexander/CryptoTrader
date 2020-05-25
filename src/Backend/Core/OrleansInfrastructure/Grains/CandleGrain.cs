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
		private readonly IPersistentState<Persistence.Entities.CandleEntity> _candle;

		public CandleGrain(
			[PersistentState(nameof(Candle), nameof(CandleStorageProvider))] IPersistentState<Persistence.Entities.CandleEntity> candle)
		{
			_candle = candle;
		}

		public async Task Set(ICandle candle)
		{
			this.GetPrimaryKey(out string keyExt);

			if (_candle.State == null)
			{
				_candle.State = new Persistence.Entities.CandleEntity(candle, keyExt.ToExtendedKey());
				await _candle.WriteStateAsync();

				IStreamProvider streamProvider = GetStreamProvider(Constants.MessageStreamProvider);
				IAsyncStream<Persistence.Entities.CandleEntity> stream = streamProvider.GetStream<Persistence.Entities.CandleEntity>(Guid.NewGuid(), nameof(Persistence.Entities.CandleEntity));
				await stream.OnNextAsync(_candle.State);
			}
		}

		public Task<ICandle> Get()
		{
			return Task.FromResult((ICandle)_candle.State);
		}
	}
}