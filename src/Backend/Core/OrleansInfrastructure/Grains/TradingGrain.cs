using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;
using Core.BusinessLogic;
using Core.Helpers;
using Orleans;
using Persistence.Helpers;

namespace Core.OrleansInfrastructure.Grains
{
    public class TradingGrain: Grain, ITradingGrain
    {
        public async Task Process()
        {
            long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
            GrainKeyExtension secondaryKey = keyExtension.ToExtended();

            ITradingContext context = new TradingContext();
            context.Exchange = (Exchanges)primaryKey;
            context.TradingPair = (secondaryKey.Asset1, secondaryKey.Asset2);

            IStrategyGrain strategyInfoGrain = GrainFactory.GetGrain<IStrategyGrain>(primaryKey, keyExtension);
            IStrategyInfo strategyInfo = await strategyInfoGrain.Get();

            context.TimeFrame = (Timeframes) strategyInfo.TimeFrame;
            
            int numberOfCandles = strategyInfo.TimeFrame * context.Strategy.MinNumberOfCandles;

            ICandleProcessingGrain candlesProcessingGrain = GrainFactory.GetGrain<ICandleProcessingGrain>(primaryKey, keyExtension);
            IEnumerable<ICandle> candles = await candlesProcessingGrain.Get(numberOfCandles);

            context.Candles = candles.GroupCandles((Timeframes) strategyInfo.TimeFrame);

            var options = OptionsHelper.Decode(strategyInfo);
            context.Strategy = StrategiesHelper.Get(strategyInfo.Class, options.options); 

            // // TODO: get asset balance
            //// decimal[] balances = Array.Empty<decimal>();
            ////
            // // TODO: Process constraints
            //
            // return context;

            TradingAdvices res = context.Strategy.Forecast(context.Candles);
        }
    }
}