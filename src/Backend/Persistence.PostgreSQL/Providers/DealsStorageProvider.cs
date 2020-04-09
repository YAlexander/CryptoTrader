using System.Threading.Tasks;
using Abstractions.Storages;
using Common;
using Orleans;
using Orleans.Runtime;
using Persistence.Entities;
using Persistence.Helpers;
using Persistence.Managers;

namespace Persistence.PostgreSQL.Providers
{
	public class DealsStorageProvider : BaseProcessor, IDealStorageProvider
	{
		private IDealsManager _dealsManager;
		
		public DealsStorageProvider(string connectionString, IDealsManager dealsManager) : base(connectionString)
		{
			_dealsManager = dealsManager;
		}

		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			grainReference.GetPrimaryKeyLong(out string keyExt);
			GrainKeyExtension secondaryKey = keyExt.ToExtendedKey();
			grainState.State = await WithConnection((connection, transaction) => _dealsManager.Get(secondaryKey.Exchange, secondaryKey.Asset1, secondaryKey.Asset2, connection, transaction));
		}

		public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			await WithConnection((connection, transaction) => _dealsManager.Update((Deal) grainState.State, connection, transaction));
		}

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			return Task.CompletedTask;
		}
	}
}