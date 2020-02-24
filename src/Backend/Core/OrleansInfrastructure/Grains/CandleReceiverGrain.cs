using System;
using System.Threading.Tasks;
using Abstractions;
using Contracts;
using Contracts.Enums;
using Core.OrleansInfrastructure.Grains.GrainStates;
using Core.OrleansInfrastructure.StorageProviders;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	public class CandleReceiverGrain : Grain, ICandleReceiver
	{
		// TODO: Move states to Orders grain
		// private readonly IPersistentState<CandleState> _candle;
		//
		// public CandleReceiver([PersistentState(nameof(candle), nameof(CandleStorage))] IPersistentState<CandleState> candle)
		// {
		// 	_candle = candle;
		// }
		
		public async Task Receive(Exchanges exchange, Assets asset1, Assets asset2, ICandle candle)
		{
			//ICandleGrain newCandle = GrainFactory.GetGrain<ICandleGrain>(Guid.NewGuid(), exchange.ToString());
			//var data = newCandle.Get(exchange, asset1, asset2, candle.Time);
			
			//await newCandle.Create(exchange, asset1, asset2, candle);
		}
	}
}