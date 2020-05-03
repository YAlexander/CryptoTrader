using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MacdSmaStrategy : BaseStrategy<MacdSmaStrategyOptions>
	{
		public override string Name { get; } = "MACD SMA Strategy";
		
		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MacdSmaStrategyOptions options = GetOptions;
			Validate(candles, options);

			var macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);
			decimal?[] fastMa = candles.Sma(options.FastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] slowMa = candles.Sma(options.SlowSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] sma = candles.Sma(options.SmaPeriod, CandleVariables.CLOSE).Result;

			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Length; i++)
			{
				// TODO: Replace with ? period
				if (i < 25)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (slowMa[i] < sma[i])
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (macd.Hist[i] > 0 && macd.Hist[i - 1] < 0 && macd.Macd[i] > 0 && fastMa[i] > slowMa[i] && closes[i - 26] > sma[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public MacdSmaStrategy(MacdSmaStrategyOptions options) : base(options)
		{
		}
	}
}
