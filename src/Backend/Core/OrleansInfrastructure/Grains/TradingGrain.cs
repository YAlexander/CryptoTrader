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
            var guid = this.GetPrimaryKey();
            var candlesStreamProvider = GetStreamProvider("SMSProvider");
            var candlesStream = candlesStreamProvider.GetStream<Candle>(guid, nameof(Candle));
            await candlesStream.SubscribeAsync<Candle>(async (data, token) =>
            {
                ITradingContext context = await BuildContext();
                
                var streamProvider = GetStreamProvider("SMSProvider");
                IAsyncStream<ITradingContext> stream = streamProvider.GetStream<ITradingContext>(guid, nameof(ITradingContext));
                await stream.OnNextAsync(context);
            });
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
            Guid primaryKey = this.GetPrimaryKey(out string keyExtension);
            GrainKeyExtension secondaryKey = keyExtension.ToExtended();

            // TODO: Check if we have open position
            
            ITradingContext context = new TradingContext();
            context.Exchange = secondaryKey.Exchange;
            context.TradingPair = (secondaryKey.Asset1, secondaryKey.Asset2);

            IStrategyGrain strategyInfoGrain = GrainFactory.GetGrain<IStrategyGrain>((int)secondaryKey.Exchange, keyExtension);
            IStrategyInfo strategyInfo = await strategyInfoGrain.Get();

            context.TimeFrame = (Timeframes) strategyInfo.TimeFrame;
            
            int numberOfCandles = strategyInfo.TimeFrame * context.Strategy.MinNumberOfCandles;

            ICandleProcessingGrain candlesProcessingGrain = GrainFactory.GetGrain<ICandleProcessingGrain>((int)secondaryKey.Exchange, keyExtension);
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