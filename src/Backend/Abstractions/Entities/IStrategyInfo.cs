using Abstractions.Enums;

namespace Abstractions.Entities
{
	public interface IStrategyInfo
	{
		Exchanges Exchange { get; set; }
		Assets Asset1Code { get; set; }
		Assets Asset2Code { get; set; }
		
		string StrategyName { get; set; }
		string StrategyClass { get; set; }
		
		string Options { get; set; }
		string DefaultOptions { get; set; }
		
		int TimeFrame { get; set; }
		
		bool IsDisabled { get; set; }
		
		string RiskManagerName { get; set; }
		string RiskManagerOptions { get; set; }
	}
}