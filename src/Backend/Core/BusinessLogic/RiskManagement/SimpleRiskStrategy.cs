using System;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;
using Core.BusinessLogic.RiskManagement.RiskStrategyOptions;

namespace Core.BusinessLogic.RiskManagement
{
	public class SimpleRiskStrategy : BaseRiskStrategy<BaseRiskOptions>
	{
		
		public override string Name { get; } = "Simple Risk Management";
		
		public override ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			if (HasAsset(context.Deal.Position == DealPositions.LONG ? context.TradingPair.asset2 : context.TradingPair.asset1, context))
			{
				
				
				return context;
			} 
			
			throw new Exception($"Not enough founds. Pair {context.TradingPair.asset1}/{context.TradingPair.asset2}");
		}

		public SimpleRiskStrategy(string options) : base(options)
		{
		}
	}
}