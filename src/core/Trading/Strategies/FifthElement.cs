using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class FifthElement : BaseStrategy
	{
		public override string Name { get; } = "5th Element";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 30;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			MacdItem macd = candles.Macd(12, 26, 9);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 4)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && macd.Hist[i] > macd.Hist[i - 1] && macd.Hist[i - 1] > macd.Hist[i - 2] && macd.Hist[i - 2] > macd.Hist[i - 3] && macd.Hist[i - 3] > macd.Hist[i - 4])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && macd.Hist[i] < macd.Hist[i - 1] && macd.Hist[i - 1] < macd.Hist[i - 2] && macd.Hist[i - 2] < macd.Hist[i - 3] && macd.Hist[i - 3] < macd.Hist[i - 4])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}
	}
}
