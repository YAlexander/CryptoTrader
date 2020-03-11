using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;
using Core.BusinessLogic;
using Core.Helpers;
using Orleans;

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
 
            // TODO: Optios decoder. (Via OptionsHelper) Strategy options will be stored in StrategyInfo
            // IStrategyOptionsManager<IStrategyOption> optionManager = _strategyOptionManagers.SingleOrDefault(x => x.StrategyName.Equals(strategyInfo.Class));
            //if (optionManager == null)
            // {
            //     throw new Exception($"Options for strategy {strategyInfo.StrategyName} wasn't found");
            //} 
            //
            
            //IStrategyOption options = await optionManager.GetOptions(exchangeCode, asset1, asset2, strategyInfo.Class);
            //context.Strategy = _strategiesHelper.Get(strategyInfo.Class, options);
            ////
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