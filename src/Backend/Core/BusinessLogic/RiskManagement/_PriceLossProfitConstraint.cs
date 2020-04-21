using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Core.BusinessLogic.TradingConstraints
{
	public class PriceLossProfitConstraint : IRiskStrategy
	{
		public string Name { get; set; } = "Stop loss / Take Profit Manager";
		
		public ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			if (context.LastTrade.HasValue)
			{
				// TODO: Use settings
				decimal gap = context.LastTrade.Value * 0.004m;
				decimal stopLossSize = context.LastTrade.Value * 0.02m;

				if (context.Deal.Position == DealPositions.LONG)
				{
					context.Price = context.LastTrade.Value + gap;
					context.StopLosePrice = context.Price - stopLossSize;
					context.TakeProfitPrice = context.Price + 3 * stopLossSize;
				}
				else
				{
					context.Price = context.LastTrade.Value - gap;
					context.StopLosePrice = context.Price + stopLossSize;
					context.TakeProfitPrice = context.Price - 3 * stopLossSize;
				}
			}

			return context;
		}
	}
}