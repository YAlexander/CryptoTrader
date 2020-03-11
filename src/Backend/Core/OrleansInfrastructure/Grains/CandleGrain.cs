﻿using System.Threading.Tasks;
using Abstractions;
using Contracts;
using Contracts.Enums;
using Orleans;
using Orleans.Runtime;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains
{
    public class CandleGrain : Grain, ICandleGrain
    {
	    private readonly IPersistentState<Candle> _candle;

	    public CandleGrain(
		    [PersistentState(nameof(candle), "cartStore")] IPersistentState<Candle> candle
		    )
	    {
		    _candle = candle;
	    }

	    public async Task<ICandle> Update(ICandle candle)
        {
            	long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
            	GrainKeyExtension secondaryKey = keyExtension.ToExtended();
            	
            	Candle newCandle = new Candle();
            	newCandle.Exchange = (Exchanges)primaryKey;
            	newCandle.Asset1 = secondaryKey.Asset1;
            	newCandle.Asset2 = secondaryKey.Asset2;
            	newCandle.Time = candle.Time;
            	newCandle.High = candle.High;
            	newCandle.Low = candle.Low;
            	newCandle.Open = candle.Open;
            	newCandle.Close = candle.Close;
            	newCandle.Volume = candle.Volume;
            	newCandle.Trades = candle.Trades;
            	newCandle.TimeFrame = candle.TimeFrame;

                _candle.State = newCandle;
                await _candle.WriteStateAsync();
                return _candle.State;
        }

        public Task<ICandle> Get()
        {
	        return Task.FromResult((ICandle)_candle.State);
        }
    }
}