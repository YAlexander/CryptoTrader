using Abstractions;

namespace Core.BusinessLogic.RiskManagement.RiskStrategyOptions
{
	public class BaseRiskOptions : IRiskStrategyOptions
	{
		public bool TradeOnFlat { get; set; }
		public bool UseTrailingStop { get; set; }
		public bool UseMarginalTrading { get; set; }
		public int Leverage { get; set; }
		public decimal MinimalOrderAmount { get; set; }
	}
}