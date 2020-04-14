using System;
using System.Linq;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Core.BusinessLogic.TradingConstraints
{
	public class InsufficientFundsConstraint : IRiskManager
	{
		// Min USD order amount
		private const decimal MinOrderAmount = 15;
		
		public string Name { get; set; } = "Insufficient Funds";
		
		public ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			IBalance funds = context.Deal.Position == DealPositions.LONG 
					? context.Funds.FirstOrDefault(x => x.Asset == context.TradingPair.asset2)
					:context.Funds.FirstOrDefault(x => x.Asset == context.TradingPair.asset1);

			decimal estimatedValue;
				
			if (context.Deal.Position == DealPositions.LONG)
			{
				estimatedValue = funds != null ? funds.TotalAmount - funds.LockedAmount : 0;
			}
			else
			{
				estimatedValue = funds != null && context.LastTrade.HasValue ? (funds.TotalAmount - funds.LockedAmount) / context.LastTrade.Value : 0;
			}
			
			if (estimatedValue >= MinOrderAmount)
			{
				return context;
			}
				
			throw new Exception($"Insufficient Funds {context.TradingPair.asset2}");
		}
	}
}