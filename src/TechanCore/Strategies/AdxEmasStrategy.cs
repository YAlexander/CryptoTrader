using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class AdxEmasStrategy : BaseStrategy<AdxEmasStrategyOptions>
	{
		public override string Name { get; } = "ADX Emas Strategy";

		public override int MinNumberOfCandles { get; } = 14;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			AdxEmasStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] smaFast = candles.Ema(options.FastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] smaSlow = candles.Ema(options.SlowSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] adx = candles.Adx(options.AdxPeriod).Adx;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else
				{
					int fastCross = smaFast[i - 1] < smaSlow[i] && smaFast[i] > smaSlow[i] ? 1 : 0;
					int slowCross = smaSlow[i - 1] < smaFast[i] && smaSlow[i] > smaFast[i] ? 1 : 0;

					if (adx[i] > 25 && fastCross == 1)
					{
						Result.Add((candles[i], TradingAdvices.BUY));
					}
					else if (adx[i] < 25 && slowCross == 1)
					{
						Result.Add((candles[i], TradingAdvices.SELL));
					}
					else
					{
						Result.Add((candles[i], TradingAdvices.HOLD));
					}
				}
			}

			return Result;
		}

		public AdxEmasStrategy(AdxEmasStrategyOptions options) : base(options)
		{
		}
	}
}
