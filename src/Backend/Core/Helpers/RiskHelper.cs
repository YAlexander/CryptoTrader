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
				switch (constraint)
				{
					case nameof(AssetPriceConstraint):
						yield return new AssetPriceConstraint();
					break;
					
					default:
						throw new Exception("Unsupported constraint");
				}
			}
		}
	}
}