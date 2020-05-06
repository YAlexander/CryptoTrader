using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class AdxMasStrategy : BaseStrategy<AdxMasStrategyOptions>
	{
		public override string Name { get; } = "ADX MA Strategy";

		public override int MinNumberOfCandles { get; } = 14;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			AdxMasStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] adx = candles.Adx(options.AdxPeriod).Adx;
			decimal?[] maFast = candles.Ma(options.MaType, options.FastMaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] maSlow = candles.Ma(options.MaType, options.SlowMaPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else
				{
					int fastCross = maFast[i - 1] < maSlow[i] && maFast[i] > maSlow[i] ? 1 : 0;
					int slowCross = maSlow[i - 1] < maFast[i] && maSlow[i] > maFast[i] ? 1 : 0;

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

		public AdxMasStrategy(AdxMasStrategyOptions options) : base(options)
		{
		}
	}
}
