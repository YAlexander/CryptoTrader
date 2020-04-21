using Abstractions;
using Abstractions.Entities;
using Core.BusinessLogic.RiskManagement.RiskStrategyOptions;

namespace Core.BusinessLogic.RiskManagement
{
	public class SimpleRiskStrategy : BaseRiskStrategy<BaseRiskOptions>
	{
		
		public override string Name { get; } = "Simple Risk Management";
		
		public override ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			throw new System.NotImplementedException();
		}

		public SimpleRiskStrategy(string options) : base(options)
		{
		}
	}
}