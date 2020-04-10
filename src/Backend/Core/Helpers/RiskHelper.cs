using System;
using System.Collections.Generic;
using Abstractions;
using Core.BusinessLogic.TradingConstraints;

namespace Core.Helpers
{
	public static class RiskHelper
	{
		public static IEnumerable<IRiskManager> Get(string[] constraints)
		{
			foreach (string constraint in constraints)
			{
				if (Managers.ContainsKey(constraint))
				{
					yield return Managers[constraint];
				}
				else
				{
					throw new Exception($"Unknown constraint {constraint}");
				}
			}
		}
		
		private static readonly Dictionary<string, IRiskManager> Managers = new Dictionary<string, IRiskManager>()
		{
			[nameof(SimpleOrderAmountConstraint)] = new SimpleOrderAmountConstraint(),
			[nameof(InsufficientFundsConstraint)] = new InsufficientFundsConstraint()
		};
	}
}