using System;
using System.Threading.Tasks;
using Abstractions.Entities;
using Abstractions.Grains;
using Orleans.Runtime;
using Persistence.Entities;
using Persistence.PostgreSQL.Providers;

namespace Core.OrleansInfrastructure.Grains
{
	public class DealGrain : IDealGrain
	{
		private readonly IPersistentState<DealEntity> _deal;
		
		public DealGrain
		(
			[PersistentState(nameof(DealEntity), nameof(DealsStorageProvider))] IPersistentState<DealEntity> deal)
		{
			_deal = deal;
		}

		public Task<IDeal> Get ()
		{
			return Task.FromResult((IDeal)_deal.State);
		}

		public async Task CreateOrUpdate (IDeal deal)
		{
			_deal.State = (DealEntity)deal;
			await _deal.WriteStateAsync();
		}
	}
}