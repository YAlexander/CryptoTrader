using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class Base150Strategy : BaseStrategy<Base150StrategyOptions>
	{
		public override string Name { get; } = "Base 150 Strategy";

		public override int MinNumberOfCandles { get; } = 365;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Base150StrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] sma6 = candles.Sma(options.VeryFastSmaPeriod, options.PriceToUse).Result;
			decimal?[] sma25 = candles.Sma(options.FastSmaPeriod, options.PriceToUse).Result;
			decimal?[] sma150 = candles.Sma(options.SlowSmaPeriod, options.PriceToUse).Result;
			decimal?[] sma365 = candles.Sma(options.VerySlowSmaPeriod, options.PriceToUse).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else
				{
					if (sma6[i] > sma150[i] && sma6[i] > sma365[i] && sma25[i] > sma150[i] && sma25[i] > sma365[i] && (sma6[i - 1] < sma150[i] || sma6[i - 1] < sma365[i] || sma25[i - 1] < sma150[i] || sma25[i - 1] < sma365[i]))
					{
						Result.Add((candles[i], TradingAdvices.BUY));
					}
					if (sma6[i] < sma150[i] && sma6[i] < sma365[i] && sma25[i] < sma150[i] && sma25[i] < sma365[i] && (sma6[i - 1] > sma150[i] || sma6[i - 1] > sma365[i] || sma25[i - 1] > sma150[i] || sma25[i - 1] > sma365[i]))
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

		public Base150Strategy(Base150StrategyOptions options) : base(options)
		{
		}
	}
}
