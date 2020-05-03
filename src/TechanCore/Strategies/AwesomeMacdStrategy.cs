using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class AwesomeMacdStrategy : BaseStrategy<AwesomeMacdStrategyOptions>
	{
		public override string Name { get; } = "Awesome MACD Strategy";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			AwesomeMacdStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] ao = candles.AwesomeOscillator(options.AwesomeFastPeriod, options.AwesomeSlowPeriod).Result;
			MacdIndicatorResult macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (ao[i] < 0 && ao[i - 1] > 0 && macd.Hist[i] < 0)
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && macd.Hist[i] > 0)
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

		public AwesomeMacdStrategy(AwesomeMacdStrategyOptions options) : base(options)
		{
		}
	}
}