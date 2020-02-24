using System.Threading.Tasks;
using Abstractions;
using Contracts;
using Contracts.Enums;
using Core.OrleansInfrastructure.Grains.GrainStates;
using Core.OrleansInfrastructure.StorageProviders;
using Orleans;
using Orleans.Runtime;

namespace Core.OrleansInfrastructure.Grains
{
	public class CandleReceiver : Grain, ICandleReceiver
	{
		private readonly IPersistentState<CandleState> _candle;

		public CandleReceiver([PersistentState(nameof(candle), nameof(CandleStorage))] IPersistentState<CandleState> candle)
		{
			_candle = candle;
		}
		
		public async Task Receive(Exchanges exchange, Assets asset1, Assets asset2, ICandle candle)
		{
			_candle.State = new CandleState
			{
				Exchange = exchange,
				Asset1 = asset1,
				Asset2 = asset2,
				Time = candle.Time,
				High = candle.High,
				Low = candle.Low,
				Open = candle.Open,
				Close = candle.Close,
				Volume = candle.Volume,
				Trades = candle.Trades
			};

			await _candle.WriteStateAsync();
		}
	}
}