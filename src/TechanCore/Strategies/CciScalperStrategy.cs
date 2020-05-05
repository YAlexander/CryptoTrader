using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class CciScalperStrategy : BaseStrategy<CciScalperStrategyOptions>
	{
		public override string Name { get; } = "CCI Scalper";

		public override int MinNumberOfCandles { get; } = 14;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			CciScalperStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] cci = candles.Cci(options.CciPeriod).Result;

			decimal?[] maFast;
			decimal?[] maNormal;
			decimal?[] maSlow;

			if (options.MaType == MaTypes.EMA)
			{
				maFast = candles.Ema(options.FastMaPeriod, CandleVariables.CLOSE).Result;
				maNormal = candles.Ema(options.NormalMaPeriod, CandleVariables.CLOSE).Result;
				maSlow = candles.Ema(options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			}
			else if (options.MaType == MaTypes.WMA)
			{
				maFast = candles.Wma(options.FastMaPeriod, CandleVariables.CLOSE).Result;
				maNormal = candles.Wma(options.NormalMaPeriod, CandleVariables.CLOSE).Result;
				maSlow = candles.Wma(options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			}
			else
			{
				maFast = candles.Sma(options.FastMaPeriod, CandleVariables.CLOSE).Result;
				maNormal = candles.Sma(options.NormalMaPeriod, CandleVariables.CLOSE).Result;
				maSlow = candles.Sma(options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			}

			for (int i = 0; i < candles.Count(); i++)
			{
				if (cci[i] < -100 && maFast[i] > maNormal[i] && maFast[i] > maSlow[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && maFast[i] < maNormal[i] && maFast[i] < maSlow[i])
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

		public CciScalperStrategy(CciScalperStrategyOptions options) : base(options)
		{
		}
	}
}
