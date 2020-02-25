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
	public class AwesomeMacd : BaseStrategy
	{
		public override string Name { get; } = "Awesome MACD";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> ao = candles.AwesomeOscillator();
			MacdItem macd = candles.Macd(5, 7, 4);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (ao[i] < 0 && ao[i - 1] > 0 && macd.Hist[i] < 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && macd.Hist[i] > 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
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