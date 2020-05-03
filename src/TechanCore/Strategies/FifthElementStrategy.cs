using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class FifthElementStrategy : BaseStrategy<FifthElementStrategyOptions>
	{
		public override string Name { get; } = "5th Element Strategy";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			FifthElementStrategyOptions options = GetOptions;
			Validate(candles, options);

			MacdIndicatorResult macd = candles.Macd(options.FastPeriod, options.SlowPeriod, options.SignalPeriod);

			for (int i = 0; i < candles.Length; i++)
			{
				if (i < 4)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && macd.Hist[i] > macd.Hist[i - 1] && macd.Hist[i - 1] > macd.Hist[i - 2] && macd.Hist[i - 2] > macd.Hist[i - 3] && macd.Hist[i - 3] > macd.Hist[i - 4])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && macd.Hist[i] < macd.Hist[i - 1] && macd.Hist[i - 1] < macd.Hist[i - 2] && macd.Hist[i - 2] < macd.Hist[i - 3] && macd.Hist[i - 3] < macd.Hist[i - 4])
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public FifthElementStrategy(FifthElementStrategyOptions options) : base(options)
		{
		}
	}
}
