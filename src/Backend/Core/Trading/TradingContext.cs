using Contracts;
using Contracts.Enums;
using Contracts.Trading;

namespace core.Trading
{
	public class TradingContext : ITradingContext
	{
		public Exchanges Exchange { get; set; }
		public (Assets asset1, Assets asset2) TradingPair { get; set; }
		public Timeframes TimeFrame { get; set; }
		public ICandle[] Candles { get; set; }
		public ITradingStrategy<IStrategyOption> Strategy { get; set; }
		public IRisk[] Risks { get; set; }
	}
}