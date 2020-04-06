using System.Threading.Tasks;
using Abstractions.Storages;
using Common;
using Orleans;
using Orleans.Runtime;

namespace Persistence.PostgreSQL.Providers
{
	public class OrdersStorageProvider : BaseProcessor, IOrderStorageProvider
	{
		public OrdersStorageProvider(DatabaseOptions options) : base(options.CryptoTradingConnectionString)
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