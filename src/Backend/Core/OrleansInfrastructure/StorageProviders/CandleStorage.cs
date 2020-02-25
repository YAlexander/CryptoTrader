using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using Persistence;

namespace Core.OrleansInfrastructure.StorageProviders
{
	public class CandleStorage : IStorageProvider
	{
		private ICandlesProcessor _candlesProcessor;
		
		public CandleStorage(ICandlesProcessor candlesProcessor)
		{
			_candlesProcessor = candlesProcessor;
		}
		
		public Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			throw new System.NotImplementedException();
		}

		public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			throw new System.NotImplementedException();
		}

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			throw new System.NotImplementedException();
		}

		public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
		{
			throw new System.NotImplementedException();
		}

		public Task Close()
		{
			throw new System.NotImplementedException();
		}

		public string Name { get; }
	}
}