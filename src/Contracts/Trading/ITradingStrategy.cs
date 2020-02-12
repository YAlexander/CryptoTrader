using Contracts.Enums;

namespace Contracts.Trading
{
	public interface ITradingStrategy
	{
		string Name { get; }
		
		int MinNumberOfCandles { get; }
		
		IStrategyOption Options { get; set; }
		
		TradingAdvices Forecast(ICandle[] candles);
	}
}
