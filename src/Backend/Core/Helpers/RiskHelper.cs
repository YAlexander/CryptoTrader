using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Core.BusinessLogic.TradingConstraints;

namespace Core.Helpers
{
	public static class RiskHelper
	{
		private static readonly IRiskManager[] ManagersOrdered =
		{
			new HasAssetsConstraint(),
			new SimpleOrderAmountConstraint(),
			new InsufficientFundsConstraint(),
			new PriceLossProfitConstraint()
		};
		
		public static IEnumerable<IRiskManager> Get(string[] constraints)
		{
			foreach (IRiskManager manager in ManagersOrdered)
			{
				if (constraints.Contains(manager.Name))
				{
					yield return manager;
				}
			}
		}
	}
}