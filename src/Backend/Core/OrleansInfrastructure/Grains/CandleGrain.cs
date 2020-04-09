using System.Threading.Tasks;
using Abstractions.Grains;
using Orleans;
using Orleans.Runtime;
using Persistence.Entities;
using Persistence.PostgreSQL.Providers;
using TechanCore;

namespace Core.OrleansInfrastructure.Grains
{
	public class CandleGrain : Grain, ICandleGrain
	{
		private readonly IPersistentState<Candle> _candle;
		
		public CandleGrain(
			[PersistentState(nameof(Candle), nameof(CandleStorageProvider))] IPersistentState<Candle> candle)
		{
			_candle = candle;
		}
		
		public async Task Set(ICandle candle)
		{
			_candle.State = (Candle) candle;
			await _candle.WriteStateAsync();
		}

		public Task<ICandle> Get()
		{
			return Task.FromResult((ICandle) _candle.State);
		}
	}
}