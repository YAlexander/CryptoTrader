using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MacdCrossStrategy : BaseStrategy<MacdCrossStrategyOptions>
	{
		public override string Name { get; } = "MACD X Strategy";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MacdCrossStrategyOptions options = GetOptions;
			Validate(candles, options);

			MacdIndicatorResult macd = candles.Macd(options.FastPeriod, options.SlowPeriod, options.SignalPeriod);
			
			for (int i = 0; i < candles.Length; i++)
			{
				bool[] crossUnder = macd.Macd.Crossunder(macd.Signal).ToArray();
				bool[] crossOver = macd.Macd.Crossover(macd.Signal).ToArray();
			
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (macd.Macd[i] > 0 && crossUnder[i])
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (macd.Macd[i] < 0 && crossOver[i])
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

		public MacdCrossStrategy(MacdCrossStrategyOptions options) : base(options)
		{
		}
	}
}
