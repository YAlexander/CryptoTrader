using TechanCore.Enums;

namespace TechanCore
{
	public interface ITradingStrategy<out T> where T : IStrategyOption
	{
		string Name { get; }
		
		int MinNumberOfCandles { get; }

		T GetOptions { get; }
		
		TradingAdvices Forecast(ICandle[] candles);
	}
}
