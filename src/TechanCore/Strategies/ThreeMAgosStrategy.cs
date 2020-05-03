using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class ThreeMAgosStrategy : BaseStrategy<ThreeMAgosStrategyOptions>
	{
		public override string Name { get; } = "Three MAgos Strategy";

		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			ThreeMAgosStrategyOptions options = GetOptions;
			Validate(candles, options);
			
			decimal?[] sma = candles.Sma(options.SmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] ema = candles.Ema(options.EmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] wma = candles.Wma(options.WmaPeriod, CandleVariables.CLOSE).Result;

			decimal[] closes = candles.Close();
			Bars[] bars = new Bars[candles.Length];

			for (int i = 0; i < candles.Length; i++)
			{
				if (closes[i] > sma[i] && closes[i] > ema[i] && closes[i] > wma[i])
				{
					bars[i] = Bars.GREEN;
				}
				else if (closes[i] > sma[i] || closes[i] > ema[i] || closes[i] > wma[i])
				{
					bars[i] = Bars.BLUE;
				}
				else
				{
					bars[i] = Bars.RED;
				}
			}

			for (int i = 0; i < candles.Length; i++)
			{
				if (i < 1)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (bars[i] == Bars.BLUE && bars[i - 1] == Bars.RED)
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (bars[i] == Bars.BLUE && bars[i - 1] == Bars.GREEN)
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

		public ThreeMAgosStrategy(ThreeMAgosStrategyOptions options) : base(options)
		{
		}
		
		private enum Bars
		{
			RED,
			BLUE,
			GREEN
		}
	}
}

