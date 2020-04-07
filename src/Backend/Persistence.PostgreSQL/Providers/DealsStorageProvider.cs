using System.Threading.Tasks;
using Abstractions.Storages;
using Orleans;
using Orleans.Runtime;

namespace Persistence.PostgreSQL.Providers
{
	public class DealsStorageProvider : BaseProcessor, IDealStorageProvider
	{
		public DealsStorageProvider(string connectionString) : base(connectionString)
		{
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