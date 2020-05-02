using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class RsiMacdStrategy : BaseStrategy <RsiMacdStrategyOptions>
	{
		public override string Name { get; } = "RSI MACD Strategy";
		
		public override int MinNumberOfCandles { get; } = 52;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiMacdStrategyOptions options = GetOptions;
			Validate(candles, options);

			MacdIndicatorResult macd = candles.Macd(options.MacdFastSmaPeriod, options.MacdSlowSmaPeriod, options.MacdSignalPeriod);
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (rsi[i] > 70 && macd.Macd[i] - macd.Signal[i] < 0)
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (rsi[i] < 30 && macd.Macd[i] - macd.Signal[i] > 0)
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

		public RsiMacdStrategy(RsiMacdStrategyOptions options) : base(options)
		{
		}
	}
}
