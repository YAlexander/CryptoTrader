using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;
using Abstractions.Grains;
using Common;
using Common.Exceptions;
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

            if (context != null)
            {
                IAsyncStream<TradingContext> stream = _streamProvider.GetStream<TradingContext>(this.GetPrimaryKey(), nameof(TradingContext));
                await stream.OnNextAsync((TradingContext)context);
            }
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

            context.TradingAdvice = context.Strategy.Forecast(context.Candles);

            if (context.TradingAdvice == TradingAdvices.HOLD)
            {
                return null;
            }
            
            keyExtension = context.ToDealExtendedKey();

            IDealGrain dealGrain = GrainFactory.GetGrain<IDealGrain>((long)context.Exchange, keyExtension.ToString());
			
            if (context.Deal == null)
            { 
                Deal deal = new Deal(); 
                deal.Id = keyExtension.Id.Value; 
                deal.Status = DealStatus.OPEN; 
                deal.Exchange = context.Exchange; 
                deal.Asset1 = context.TradingPair.asset1; 
                deal.Asset2 = context.TradingPair.asset2; 
                deal.Position = context.TradingAdvice == TradingAdvices.BUY ? DealPositions.LONG : DealPositions.SHORT;
                
                await dealGrain.CreateOrUpdate(deal);
            }

            context.Deal = await dealGrain.Get();
            
            IBalanceProcessingGrain balanceGrain = GrainFactory.GetGrain<IBalanceProcessingGrain>((long)exchange);
            IEnumerable<IBalance> balances = await balanceGrain.Get();

            context.Funds = balances.ToList();

            IEnumerable<IRiskManager> riskManagers = RiskHelper.Get(strategyInfo.Constraints);

            ITradeProcessingGrain tradesGrain = GrainFactory.GetGrain<ITradeProcessingGrain>((long)keyExtension.Exchange);
            context.LastTrade = await tradesGrain.Get(keyExtension.Asset1, keyExtension.Asset2);
            
            try
            {
                foreach (IRiskManager manager in riskManagers)
                {
                    manager.Process(context, strategyInfo);
                }
            }
            catch (RiskManagementException ex)
            {
                // TODO: Log error
                return null;
            }

            return context;
        }

        public Task<ITradingContext> GetContext(Exchanges exchange, Assets asset1, Assets asset2)
        {
            return BuildContext(exchange, asset1, asset2);
        }
    }
}