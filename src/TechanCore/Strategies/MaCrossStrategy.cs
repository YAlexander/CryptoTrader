using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MaCrossStrategy : BaseStrategy<MaCrossStrategyOptions>
	{
		public override string Name { get; } = "MA Cross Strategy";

		public override int MinNumberOfCandles { get; } = 36;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MaCrossStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] maShort = candles.Ma(options.MaType, options.FastMaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] maLong = candles.Ma(options.MaType, options.SlowMaPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (maShort[i] < maLong[i] && maShort[i - 1] > maLong[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (maShort[i] > maLong[i] && maShort[i - 1] < maLong[i])
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

		public MaCrossStrategy(MaCrossStrategyOptions options) : base(options)
		{
		}
	}
}
