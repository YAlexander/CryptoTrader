using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Common;
using Contracts;
using Contracts.Enums;
using Orleans;
using Orleans.Concurrency;
using Persistence;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	[ImplicitStreamSubscription(nameof(Candle))]
	public class CandleProcessingGrain : Grain, ICandleProcessingGrain
	{
		private readonly ICandlesProcessor _candlesProcessor;
		
		public CandleProcessingGrain(ICandlesProcessor candlesProcessor)
		{
			_candlesProcessor = candlesProcessor;
		}
		
		public Task<IEnumerable<ICandle>> Get(DateTime from, DateTime to)
		{
			long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
			GrainKeyExtension secondaryKey = keyExtension.ToExtended();

			return _candlesProcessor.GetCandles((Exchanges)primaryKey, secondaryKey.Asset1, secondaryKey.Asset2, from, to);
		}

		public Task<IEnumerable<ICandle>> Get(int numberOfLastCandles)
		{
			long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
			GrainKeyExtension secondaryKey = keyExtension.ToExtended();

			return _candlesProcessor.GetCandles((Exchanges)primaryKey, secondaryKey.Asset1, secondaryKey.Asset2, numberOfLastCandles);
		}
	}
}