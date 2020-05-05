using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class CciMaStrategy : BaseStrategy<CciMaStrategyOptions>
	{
		public override string Name { get; } = "CCI MA Strategy";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			CciMaStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] cci = candles.Cci(options.CciPeriod).Result;
			decimal?[] maFast;
			decimal?[] maSlow;

			if (options.MaType == MaTypes.EMA)
			{
				maFast = candles.Ema(options.MaFast, CandleVariables.CLOSE).Result;
				maSlow = candles.Ema(options.MaSlow, CandleVariables.CLOSE).Result;
			}
			else if (options.MaType == MaTypes.WMA)
			{
				maFast = candles.Wma(options.MaFast, CandleVariables.CLOSE).Result;
				maSlow = candles.Wma(options.MaSlow, CandleVariables.CLOSE).Result;
			}
			else
			{
				maFast = candles.Sma(options.MaFast, CandleVariables.CLOSE).Result;
				maSlow = candles.Sma(options.MaSlow, CandleVariables.CLOSE).Result;
			}

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (cci[i] < -100 && maFast[i] > maSlow[i] && maFast[i - 1] < maSlow[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && maFast[i] < maSlow[i] && maFast[i - 1] > maSlow[i])
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

		public CciMaStrategy(CciMaStrategyOptions options) : base(options)
		{
		}
	}
}
