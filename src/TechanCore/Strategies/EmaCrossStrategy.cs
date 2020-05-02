using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class EmaCrossStrategy : BaseStrategy<EmaCrossStrategyOptions>
	{
		public override string Name { get; } = "EMA Cross Strategy";

		public override int MinNumberOfCandles { get; } = 36;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			EmaCrossStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] emaShort = candles.Ema(options.FastEmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] emaLong = candles.Ema(options.SlowEmaPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (emaShort[i] < emaLong[i] && emaShort[i - 1] > emaLong[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (emaShort[i] > emaLong[i] && emaShort[i - 1] < emaLong[i])
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

		public EmaCrossStrategy(EmaCrossStrategyOptions options) : base(options)
		{
		}
	}
}
