using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class FifthElement : BaseStrategy
	{
		public override string Name { get; } = "5th Element";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdItem macd = candles.Macd(12, 26, 9);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 4)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && macd.Hist[i] > macd.Hist[i - 1] && macd.Hist[i - 1] > macd.Hist[i - 2] && macd.Hist[i - 2] > macd.Hist[i - 3] && macd.Hist[i - 3] > macd.Hist[i - 4])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && macd.Hist[i] < macd.Hist[i - 1] && macd.Hist[i - 1] < macd.Hist[i - 2] && macd.Hist[i - 2] < macd.Hist[i - 3] && macd.Hist[i - 3] < macd.Hist[i - 4])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}
