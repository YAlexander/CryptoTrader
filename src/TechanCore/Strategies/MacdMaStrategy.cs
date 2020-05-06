using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MacdMaStrategy : BaseStrategy<MacdMaStrategyOptions>
	{
		public override string Name { get; } = "MACD SMA Strategy";
		
		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MacdMaStrategyOptions options = GetOptions;
			Validate(candles, options);

			var macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);

			decimal?[] fastMa = candles.Ma(options.MaType, options.FastMaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] slowMa = candles.Ma(options.MaType, options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] normalMa = candles.Ma(options.MaType, options.NormalMaPeriod, CandleVariables.CLOSE).Result;

			decimal[] closes = candles.Close();

			for (int i = 0; i < candles.Length; i++)
			{
				// TODO: Replace with ? period
				if (i < 25)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (slowMa[i] < normalMa[i])
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (macd.Hist[i] > 0 && macd.Hist[i - 1] < 0 && macd.Macd[i] > 0 && fastMa[i] > slowMa[i] && closes[i - 26] > normalMa[i])
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

		public MacdMaStrategy(MacdMaStrategyOptions options) : base(options)
		{
		}
	}
}
