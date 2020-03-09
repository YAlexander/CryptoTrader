using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Contracts;
using Contracts.Enums;
using Orleans;
using Orleans.Concurrency;
using Persistence;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	public class CandleProcessingGrain : Grain, ICandleProcessingGrain
	{
		private readonly ICandlesProcessor _candlesProcessor;
		
		public CandleProcessingGrain(ICandlesProcessor candlesProcessor)
		{
			_candlesProcessor = candlesProcessor;
		}
		
		public async Task<long?> Create(ICandle candle)
		{
			long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
			GrainKeyExtension secondaryKey = keyExtension.ToExtended();
			
			Candle newCandle = new Candle();
			newCandle.Exchange = (Exchanges)primaryKey;
			newCandle.Asset1 = secondaryKey.Asset1;
			newCandle.Asset2 = secondaryKey.Asset2;
			newCandle.Time = candle.Time;
			newCandle.High = candle.High;
			newCandle.Low = candle.Low;
			newCandle.Open = candle.Open;
			newCandle.Close = candle.Close;
			newCandle.Volume = candle.Volume;
			newCandle.Trades = candle.Trades;
			newCandle.TimeFrame = candle.TimeFrame;
			
			return await _candlesProcessor.Create(newCandle);
		}

		public async Task<IEnumerable<ICandle>> Get(DateTime from, DateTime to)
		{
			long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
			GrainKeyExtension secondaryKey = keyExtension.ToExtended();

			return await _candlesProcessor.GetCandles((Exchanges)primaryKey, secondaryKey.Asset1, secondaryKey.Asset2, from, to);
		}

		public async Task<IEnumerable<ICandle>> Get(int numberOfLastCandles)
		{
			long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
			GrainKeyExtension secondaryKey = keyExtension.ToExtended();

			return await _candlesProcessor.GetCandles((Exchanges)primaryKey, secondaryKey.Asset1, secondaryKey.Asset2, numberOfLastCandles);
		}
	}
}