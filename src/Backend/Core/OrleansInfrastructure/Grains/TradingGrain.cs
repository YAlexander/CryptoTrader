using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Common;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;
using Core.BusinessLogic;
using Core.Helpers;
using Orleans;
using Orleans.Streams;
using Persistence.Entities;
using Persistence.Helpers;

namespace Core.OrleansInfrastructure.Grains
{
    [ImplicitStreamSubscription(nameof(Candle))]
    public class TradingGrain: Grain, ITradingGrain, IAsyncObserver<Candle>
    {
        public async Task<ITradingContext> GetContext()
        {
            return await BuildContext();
        }
        
        public async Task OnNextAsync(Candle item, StreamSequenceToken token = null)
        {
            ITradingContext context = await BuildContext();
            
            IStreamProvider streamProvider = this.GetStreamProvider("SMSProvider");
            IAsyncStream<ITradingContext> stream = streamProvider.GetStream<ITradingContext>(Guid.NewGuid(), nameof(ITradingContext));
            await stream.OnNextAsync(context);
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        private async Task<ITradingContext> BuildContext()
        {
            long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
            GrainKeyExtension secondaryKey = keyExtension.ToExtended();

            // TODO: Check if we have open position
            
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

            (IStrategyOption options, IStrategyOption defaultOptions) options = OptionsHelper.Decode(strategyInfo);
            context.Strategy = StrategiesHelper.Get(strategyInfo.Class, options.options); 

            // // TODO: get asset balance
            //// decimal[] balances = Array.Empty<decimal>();
            ////
            // // TODO: Process constraints
            //
            // return context;

            context.TradingAdvice = context.Strategy.Forecast(context.Candles);
            return context;
        }
    }
}