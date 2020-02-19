using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Helpers;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MacdCross : BaseStrategy<MacdCrossStrategyOptions>
	{
		public override string Name { get; } = "MACD X";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			MacdCrossStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdIndicatorResult macd = candles.Macd(options.FastPeriod, options.SlowPeriod, options.SignalPeriod);
			
			for (int i = 0; i < candles.Length; i++)
			{
				bool[] crossUnder = macd.Macd.Crossunder(macd.Signal).ToArray();
				bool[] crossOver = macd.Macd.Crossover(macd.Signal).ToArray();
			
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (macd.Macd[i] > 0 && crossUnder[i])
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (macd.Macd[i] < 0 && crossOver[i])
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

		public MacdCross(MacdCrossStrategyOptions options) : base(options)
		{
		}
	}
}
