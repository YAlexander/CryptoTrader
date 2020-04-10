using Abstractions;
using Abstractions.Entities;

namespace Core.BusinessLogic.TradingConstraints
{
	public class InsufficientFundsConstraint : IRiskManager
	{
		public string Name { get; set; } = "Insufficient Funds";
		public ITradingContext Process(ITradingContext context, IStrategyInfo info)
		{
			throw new System.NotImplementedException();
		}
	}
}