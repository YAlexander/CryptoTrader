using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class AwesomeMaStrategy : BaseStrategy<AwesomeMaStrategyOptions>
	{
		public override string Name { get; } = "Awesome MA Strategy";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			AwesomeMaStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] ao = candles.AwesomeOscillator(options.AwesomeFastPeriod, options.AwesomeSlowPeriod).Result;

			decimal?[] maShort;
			decimal?[] maLong;

			if (options.MaType == MaTypes.EMA)
			{
				maShort = candles.Ema(options.MaFastPeriod, CandleVariables.CLOSE).Result;
				maLong = candles.Ema(options.MaSlowPeriod, CandleVariables.CLOSE).Result;
			}
			else if (options.MaType == MaTypes.WMA)
			{
				maShort = candles.Wma(options.MaFastPeriod, CandleVariables.CLOSE).Result;
				maLong = candles.Wma(options.MaSlowPeriod, CandleVariables.CLOSE).Result;
			}
			else
			{
				maShort = candles.Sma(options.MaFastPeriod, CandleVariables.CLOSE).Result;
				maLong = candles.Sma(options.MaSlowPeriod, CandleVariables.CLOSE).Result;
			}

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && maShort[i] > maLong[i] || ao[i] > 0 && maShort[i] > maLong[i] && maShort[i - 1] < maLong[i - 1])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (maShort[i] < maLong[i] && maShort[i - 1] > maLong[i - 1])
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

		public AwesomeMaStrategy(AwesomeMaStrategyOptions options) : base(options)
		{
		}
	}
}