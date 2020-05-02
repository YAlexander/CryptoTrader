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

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			DoubleVolatilityStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] smaFastHigh = candles.Sma(options.FastSmaPeriod, CandleVariables.HIGH).Result;
			decimal?[] smaNormalHigh = candles.Sma(options.NormalSmaPeriod, CandleVariables.HIGH).Result;
			decimal?[] smaSlowLow = candles.Sma(options.SlowSmaPeriod, CandleVariables.LOW).Result;
			decimal[] closes = candles.Close();
			decimal[] opens = candles.Open();
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i < 1)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (smaFastHigh[i] > smaNormalHigh[i] 
						&& rsi[i] > 65
						&& Math.Abs(opens[i - 1] - closes[i - 1]) > 0 
						&& Math.Abs(opens[i] - closes[i]) / Math.Abs(opens[i - 1] - closes[i - 1]) < 2)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (smaFastHigh[i] < smaSlowLow[i] && rsi[i] < 35)
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public DoubleVolatilityStrategy(DoubleVolatilityStrategyOptions options) : base(options)
		{
		}
	}
}
