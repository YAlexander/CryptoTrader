using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MomentumStrategy : BaseStrategy<MomentumStrategyOptions>
	{
		public override string Name { get; } = "Momentum Strategy";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MomentumStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] mom = candles.Momentum(options.MomentumPeriod, CandleVariables.CLOSE).Result;
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;
			decimal[] closes = candles.Select(x => x.Close).ToArray();

			decimal?[] maFast;
			decimal?[] maSlow;

			if (options.MaType == MaTypes.EMA)
			{
				maFast = candles.Ema(options.FastMaPeriod, CandleVariables.CLOSE).Result;
				maSlow = candles.Ema(options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			}
			else if (options.MaType == MaTypes.WMA)
			{
				maFast = candles.Wma(options.FastMaPeriod, CandleVariables.CLOSE).Result;
				maSlow = candles.Wma(options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			}
			else
			{
				maFast = candles.Sma(options.FastMaPeriod, CandleVariables.CLOSE).Result;
				maSlow = candles.Sma(options.SlowMaPeriod, CandleVariables.CLOSE).Result;
			}

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 30 && mom[i] > 0 && maFast[i] > maSlow[i] && closes[i] > maSlow[i] && closes[i] > maFast[i])
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (rsi[i] > 70 && mom[i] < 0 && maFast[i] < maSlow[i] && closes[i] < maSlow[i] && closes[i] < maFast[i])
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public MomentumStrategy(MomentumStrategyOptions options) : base(options)
		{
		}
	}
}