using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MaAdxMacdStrategy : BaseStrategy<MaAdxMacdOptions>
	{
		public override string Name { get; } = "MA ADX MACD";
		
		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MaAdxMacdOptions options = GetOptions;
			Validate(candles, options);

			AdxResult adx = candles.Adx(options.AdxPeriod);
			decimal?[] maFast = candles.Ma(options.MaType, options.FastMaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] maSlow = candles.Ma(options.MaType, options.SlowMaPeriod, CandleVariables.CLOSE).Result;

			MacdIndicatorResult macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (maFast[i] < maSlow[i] && maFast[i - 1] > maSlow[i] && macd.Macd[i] < 0 && adx.PlusDi[i] > adx.MinusDi[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (maFast[i] > maSlow[i] && maFast[i - 1] < maSlow[i] && macd.Macd[i] > 0 && adx.PlusDi[i] < adx.MinusDi[i])
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

		public MaAdxMacdStrategy(MaAdxMacdOptions options) : base(options)
		{
		}
	}
}
