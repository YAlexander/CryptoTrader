using System.Collections.Generic;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class RsiSmaCrossoverStrategy : BaseStrategy<RsiSmaCrossoverStrategyOptions>
	{
		public override string Name { get; } = "RSI - SMA Crossover Strategy";

		public override int MinNumberOfCandles { get; } = 60;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiSmaCrossoverStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] smaFast = candles.Sma(options.SmaFastPeriod, CandleVariables.CLOSE).Result;
			decimal?[] smaSlow = candles.Sma(options.SmaSlowPeriod, CandleVariables.CLOSE).Result;
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			decimal startRsi = 0m;

			for (int i = 0; i < candles.Length; i++)
			{
				if (rsi[i] > startRsi)
				{
					startRsi = rsi[i].Value;
				}

				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (startRsi - rsi[i] > 10)
				{
					startRsi = 0;
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (smaFast[i] > smaSlow[i] && smaFast[i - 1] < smaSlow[i - 1] && rsi[i] <= 65)
				{
					startRsi = rsi[i].Value;
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public RsiSmaCrossoverStrategy(RsiSmaCrossoverStrategyOptions options) : base(options)
		{
		}
	}
}
