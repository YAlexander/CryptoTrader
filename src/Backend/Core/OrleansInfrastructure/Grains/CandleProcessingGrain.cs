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
		
		public async Task<long?> Create(Exchanges exchange, Assets asset1, Assets asset2, ICandle candle)
		{
			Candle newCandle = new Candle();
			newCandle.Exchange = exchange;
			newCandle.Asset1 = asset1;
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

		public async Task<IEnumerable<ICandle>> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to)
		{
			return await _candlesProcessor.GetCandles(exchange, asset1, asset2, from, to);
		}

		public Task<IEnumerable<ICandle>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles)
		{
			throw new NotImplementedException();
		}
	}
}