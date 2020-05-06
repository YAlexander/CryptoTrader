using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class RsiSmaCrossoverStrategy : BaseStrategy<RsiMaCrossoverStrategyOptions>
	{
		public override string Name { get; } = "RSI - SMA Crossover Strategy";

		public override int MinNumberOfCandles { get; } = 60;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			RsiMaCrossoverStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] maFast = candles.Ma(options.MaType, options.MaFastPeriod, CandleVariables.CLOSE).Result;
			decimal?[] maSlow = candles.Ma(options.MaType, options.MaSlowPeriod, CandleVariables.CLOSE).Result;
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			decimal startRsi = 0m;

			for (int i = 0; i < candles.Length; i++)
			{
				if (rsi[i] > startRsi)
				{
					startRsi = rsi[i].Value;
				}

				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (startRsi - rsi[i] > 10)
				{
					startRsi = 0;
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (maFast[i] > maSlow[i] && maFast[i - 1] < maSlow[i - 1] && rsi[i] <= 65)
				{
					startRsi = rsi[i].Value;
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public RsiSmaCrossoverStrategy(RsiMaCrossoverStrategyOptions options) : base(options)
		{
		}
	}
}
