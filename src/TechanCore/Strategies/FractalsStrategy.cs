using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class FractalsStrategy : BaseStrategy<FractalsStrategyOptions>
	{
		public override string Name { get; } = "Fractals";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			FractalsStrategyOptions options = GetOptions;
			Validate(candles, options);

			// Our lists to hold our values
			List<decimal?> fractalPrice = new List<decimal?>();
			List<decimal?> fractalAverage = new List<decimal?>();
			List<bool> fractalTrend = new List<bool>();

			decimal?[] ao = candles.AwesomeOscillator(options.AwesomeFastPeriod, options.AwesomeSlowPeriod).Result;
			decimal[] high = candles.High();
			decimal[] highLowAvgs = candles.AverageExtremum();

			for (int i = 0; i < candles.Length; i++)
			{
				// Calculate the price for this fractal
				if (i < 4)
				{
					fractalPrice.Add(0);
					fractalAverage.Add(0);
					fractalTrend.Add(false);
					Result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					bool fractalTop = high[i - 2] > high[i - 3] &&
									 high[i - 2] > high[i - 4] &&
									 high[i - 2] > high[i - 1] &&
									 high[i - 2] > high[i];
					decimal? price = fractalTop ? highLowAvgs[i] : 0;
					fractalPrice.Add(price);

					// Calculate the avg price
					decimal? avg = (fractalPrice[i - 1] + fractalPrice[i - 2] + fractalPrice[i - 3]) / 3;
					fractalAverage.Add(avg);

					// Check the trend.
					bool trend = fractalAverage[i] > fractalAverage[i - 1];
					fractalTrend.Add(trend);

					bool fractalBreakout = highLowAvgs[i - 1] > fractalPrice[i];

					bool tradeEntry = fractalTrend[i] && fractalBreakout;
					bool tradeExit = fractalTrend[i - options.ExitAfterBarsCount] && fractalTrend[i] == false;

					if (tradeExit)
					{
						Result.Add((candles[i], TradingAdvices.SELL));
					}
					else if (tradeEntry && ao[i] > 0)
					{
						Result.Add((candles[i], TradingAdvices.BUY));
					}
					else
					{
						Result.Add((candles[i], TradingAdvices.HOLD));
					}
				}
			}

			return Result;
		}

		public FractalsStrategy(FractalsStrategyOptions options) : base(options)
		{
		}
	}
}