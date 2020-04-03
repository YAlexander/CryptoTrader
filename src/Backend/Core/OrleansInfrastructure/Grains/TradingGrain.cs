﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Enums;
using Common;
using Core.BusinessLogic;
using Core.Helpers;
using Orleans;
using Orleans.Concurrency;
using Orleans.Streams;
using Persistence.Entities;
using Persistence.Helpers;
using TechanCore;
using TechanCore.Enums;

namespace Core.OrleansInfrastructure.Grains
{
    [StatelessWorker]
    [ImplicitStreamSubscription(nameof(Candle))]
    public class TradingGrain: Grain, ITradingGrain
    {
        private IStreamProvider _streamProvider; 
        
        public override async Task OnActivateAsync()
        {
            _streamProvider = GetStreamProvider(Constants.MessageStreamProvider);
            IAsyncStream<Candle> stream = _streamProvider.GetStream<Candle>(this.GetPrimaryKey(), nameof(Candle));
            await stream.SubscribeAsync(OnNextAsync);
        }

        private async Task OnNextAsync(Candle item, StreamSequenceToken token = null)
        {
            ITradingContext context = await BuildContext(item.Exchange, item.Asset1, item.Asset2);
            IAsyncStream<TradingContext> stream = _streamProvider.GetStream<TradingContext>(this.GetPrimaryKey(), nameof(TradingContext));
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        private async Task<ITradingContext> BuildContext(Exchanges exchange, Assets asset1, Assets asset2)
        {
            GrainKeyExtension keyExtension = new GrainKeyExtension();
            keyExtension.Exchange = exchange;
            keyExtension.Asset1 = asset1;
            keyExtension.Asset2 = asset2;

                // TODO: Check if we have open position
            
            ITradingContext context = new TradingContext();
            context.Exchange = exchange;
            context.TradingPair = (asset1, asset2);

            IStrategyGrain strategyInfoGrain = GrainFactory.GetGrain<IStrategyGrain>((int)exchange, keyExtension.ToString());
            IStrategyInfo strategyInfo = await strategyInfoGrain.Get();

            context.TimeFrame = (Timeframes) strategyInfo.TimeFrame;
            
            int numberOfCandles = strategyInfo.TimeFrame * context.Strategy.MinNumberOfCandles;

            ICandleProcessingGrain candlesProcessingGrain = GrainFactory.GetGrain<ICandleProcessingGrain>((int)exchange, keyExtension.ToString());
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

        public Task<ITradingContext> GetContext(Exchanges exchange, Assets asset1, Assets asset2)
        {
            return BuildContext(exchange, asset1, asset2);
        }
    }
}