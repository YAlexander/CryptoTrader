using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Core.BusinessLogic.TradingConstraints
{
	public class PriceLossProfitConstraint : IRiskManager
	{
		public string Name { get; set; } = "Stop loss / Take Profit Manager";
		
		public ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			if (context.LastTrade.HasValue)
			{
				// TODO: Use settings
				decimal gap = context.LastTrade.Value * 0.004m;
				decimal stopLossSize = context.LastTrade.Value * 0.02m;

				decimal limitPrice;
				decimal stopLossPrice;
				decimal takeProfitPrice;

				if (context.Deal.Position == DealPositions.LONG)
				{
					limitPrice = context.LastTrade.Value + gap;
					stopLossPrice = limitPrice - stopLossSize;
					takeProfitPrice = limitPrice + 3 * stopLossSize;
				}
				else
				{
					limitPrice = context.LastTrade.Value - gap;
					stopLossPrice = limitPrice + stopLossSize;
					takeProfitPrice = limitPrice - 3 * stopLossSize;
				}

				context.Price = limitPrice;
				context.StopLosePrice = stopLossPrice;
				context.TakeProfitPrice = takeProfitPrice;
			}

			return context;
		}
	}
}