using System;
using System.Linq;
using Abstractions;
using Abstractions.Entities;

namespace Core.BusinessLogic.TradingConstraints
{
	public class HasAssetsConstraint : IRiskManager
	{
		public string Name { get; set; } = "Has Pair Assets";
		public ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			IBalance[] funds = context.Funds
				.Where(x => x.TotalAmount - x.LockedAmount > 0 && (x.Asset == context.TradingPair.asset1 || x.Asset == context.TradingPair.asset2))
				.ToArray();

			if (funds.Length != 2)
			{
				throw new Exception($"You haven't {context.TradingPair.asset1}, {context.TradingPair.asset2} asset(s)");
			}

			return context;
		}
	}
}