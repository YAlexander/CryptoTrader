using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class BreakoutMaStrategy : BaseStrategy<BreakoutMaOptions>
	{
		public override string Name { get; } = "Breakout MA Strategy";

		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			BreakoutMaOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] sma = candles.Sma(options.SmaPeriod,CandleVariables.CLOSE).Result;
			decimal?[] ema = candles.Ema(options.EmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] adx = candles.Adx(options.AdxPeriod).Adx;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (ema[i - 1] > sma[i - 1] && ema[i] < sma[i] && adx[i] > 25)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (ema[i] > sma[i] && ema[i - 1] < sma[i - 1] && adx[i] > 25)
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

		public BreakoutMaStrategy(BreakoutMaOptions options) : base(options)
		{
		}
	}
}
