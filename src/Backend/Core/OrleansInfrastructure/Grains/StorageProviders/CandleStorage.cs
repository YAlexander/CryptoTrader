using System;
using System.Threading.Tasks;
using Orleans.Runtime;
using Persistence;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	public class CandleStorage : IPersistentState<Candle>
	{
		private readonly ICandlesProcessor _candlesProcessor;
		
		public CandleStorage(ICandlesProcessor candlesProcessor)
		{
			_candlesProcessor = candlesProcessor;
		}
			
		public Task ClearStateAsync()
		{
			throw new NotImplementedException();
		}

		public Task WriteStateAsync()
		{
			_candlesProcessor.Create(State);
			return Task.CompletedTask;
		}

		public Task ReadStateAsync()
		{
			throw new NotImplementedException();
		}

		public Candle State { get; set; }
		public string Etag { get; }
	}
}