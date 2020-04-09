using System.Threading.Tasks;
using Abstractions.Storages;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;

namespace Core.OrleansInfrastructure.Grains.StorageProviders
{
	public class DealStorage : IGrainStorage
	{
		private IDealStorageProvider _dealsProcessor;
		
		public DealStorage(IDealStorageProvider dealsProcessor)
		{
			_dealsProcessor = dealsProcessor;
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
	}
}