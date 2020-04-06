using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Enums;
using Abstractions.Grains;
using Common;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using Orleans.Streams;
using Persistence;
using Persistence.Entities;
using TechanCore;

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