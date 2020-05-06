using System;
using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class DoubleVolatilityStrategy : BaseStrategy<DoubleVolatilityStrategyOptions>
	{
		public override string Name { get; } = "Double Volatility Strategy";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			DoubleVolatilityStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal[] closes = candles.Close();
			decimal[] opens = candles.Open();
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			decimal?[] maFastHigh = candles.Ma(options.MaType, options.FastMaPeriod, CandleVariables.HIGH).Result;
			decimal?[] maNormalHigh = candles.Ma(options.MaType, options.NormalMaPeriod, CandleVariables.HIGH).Result;
			decimal?[] maSlowLow = candles.Ma(options.MaType, options.SlowMaPeriod, CandleVariables.HIGH).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i < 1)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (maFastHigh[i] > maNormalHigh[i]
						&& rsi[i] > 65
						&& Math.Abs(opens[i - 1] - closes[i - 1]) > 0
						&& Math.Abs(opens[i] - closes[i]) / Math.Abs(opens[i - 1] - closes[i - 1]) < 2)
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (maFastHigh[i] < maSlowLow[i] && rsi[i] < 35)
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

		public DoubleVolatilityStrategy(DoubleVolatilityStrategyOptions options) : base(options)
		{
		}
	}
}
