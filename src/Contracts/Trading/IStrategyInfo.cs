using Contracts.Enums;

namespace Contracts.Trading
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
		
		string Class { get; set; }
		int TimeFrame { get; set; }

		bool TradeOnFlat { get; set; }
		
		bool UseTrailingStop { get; set; }
		
		bool IsStopLossPercent { get; set; }
		decimal? StopLossValue { get; set; }

		bool IsTakeProfitPercent { get; set; }
		decimal? TakeProfitValue { get; set; }
		
		bool UseMarginalTrading { get; set; }
		int Leverage { get; set; }
		
		bool IsDisabled { get; set; }
	}
}