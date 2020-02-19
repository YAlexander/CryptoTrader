using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MacdSma : BaseStrategy<MacdSmaStrategyOptions>
	{
		public override string Name { get; } = "MACD SMA";
		
		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			MacdSmaStrategyOptions options = GetOptions;
			Validate(candles, options);
			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			var macd = candles.Macd(options.FastPeriod, options.SlowPeriod, options.SignalPeriod);
			decimal?[] fastMa = candles.Sma(options.FastPeriod, CandleVariables.CLOSE).Result;
			decimal?[] slowMa = candles.Sma(options.SlowPeriod, CandleVariables.CLOSE).Result;
			decimal?[] sma = candles.Sma(options.SmaPeriod, CandleVariables.CLOSE).Result;

			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Length; i++)
			{
				// TODO: Replace with ? period
				if (i < 25)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (slowMa[i] < sma[i])
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (macd.Hist[i] > 0 && macd.Hist[i - 1] < 0 && macd.Macd[i] > 0 && fastMa[i] > slowMa[i] && closes[i - 26] > sma[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public MacdSma(MacdSmaStrategyOptions options) : base(options)
		{
		}
	}
}
