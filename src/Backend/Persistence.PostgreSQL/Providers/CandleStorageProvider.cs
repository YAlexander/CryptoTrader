using System.Threading.Tasks;
using Abstractions.Storages;
using Common;
using Orleans;
using Orleans.Runtime;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.Providers
{
	public class CandleStorageProvider : BaseProcessor, ICandleStorageProvider
	{
		private readonly ICandlesManager _candlesManager;
		
		public CandleStorageProvider(string connectionString, ICandlesManager candlesManager) : base(connectionString)
		{
			_candlesManager = candlesManager;
		}

		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			grainReference.GetPrimaryKeyLong(out string keyExt);
			GrainKeyExtension secondaryKey = keyExt.ToExtended();
			
			grainState.State = await WithConnection((connection, transaction) => _candlesManager.Get(secondaryKey.Exchange, secondaryKey.Asset1, secondaryKey.Asset2, secondaryKey.Time, connection, transaction));
		}

		public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			await WithConnection((connection, transaction) => _candlesManager.Create((Candle) grainState.State, connection, transaction));
		}

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			return Task.CompletedTask;
		}
	}
}