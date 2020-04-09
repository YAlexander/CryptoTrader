using Abstractions.Grains;
using Orleans.Runtime;
using Persistence.Entities;
using Persistence.PostgreSQL.Providers;

namespace Core.OrleansInfrastructure.Grains
{
	public class DealGrain : IDealGrain
	{
		private readonly IPersistentState<Deal> _deal;
		
		public DealGrain
		(
			[PersistentState(nameof(Deal), nameof(DealsStorageProvider))] IPersistentState<Deal> deal)
		{
			_deal = deal;
		}
	}
}