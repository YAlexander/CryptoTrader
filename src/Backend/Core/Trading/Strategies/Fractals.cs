using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class Fractals : BaseStrategy
	{
		public override string Name { get; } = "Fractals";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			// Settings for this strategy
			int exitAfterBars = 3;
			bool useLongerAverage = true;
			bool noRepainting = true;

			// Our lists to hold our values
			List<decimal> fractalPrice = new List<decimal>();
			List<decimal> fractalAverage = new List<decimal>();
			List<bool> fractalTrend = new List<bool>();

			List<decimal?> ao = candles.AwesomeOscillator();
			List<decimal> high = candles.Select(x => x.High).ToList();
			List<decimal> highLowAvgs = candles.Select(x => (x.High + x.Low) / 2).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				// Calculate the price for this fractal
				if (i < 4)
				{
					fractalPrice.Add(0);
					fractalAverage.Add(0);
					fractalTrend.Add(false);
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					bool fractalTop = high[i - 2] > high[i - 3] &&
									 high[i - 2] > high[i - 4] &&
									 high[i - 2] > high[i - 1] &&
									 high[i - 2] > high[i];
					decimal price = fractalTop ? highLowAvgs[i] : 0;
					fractalPrice.Add(price);

					// Calculate the avg price
					decimal avg = useLongerAverage
						? (fractalPrice[i - 1] + fractalPrice[i - 2] + fractalPrice[i - 3]) / 3
						: (fractalPrice[i - 1] + fractalPrice[i - 2]) / 2;
					fractalAverage.Add(avg);

					// Check the trend.
					bool trend = fractalAverage[i] > fractalAverage[i - 1];
					fractalTrend.Add(trend);

					bool fractalBreakout = noRepainting
						? highLowAvgs[i - 1] > fractalPrice[i]
						: highLowAvgs[i] > fractalPrice[i];

					bool tradeEntry = fractalTrend[i] && fractalBreakout;
					bool tradeExit = fractalTrend[i - exitAfterBars] && fractalTrend[i] == false;

					if (tradeExit)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
					else if (tradeEntry && ao[i] > 0)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}
	}
}