using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class SimpleBearBull : BaseStrategy
	{
		public override string Name { get; } = "The Bull and The Bear";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 5;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();
			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i >= 2)
				{
					decimal current = closes[i];
					decimal previous = closes[i - 1];
					decimal prior = closes[i - 2];

					if (current > previous && previous > prior)
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (current < previous)
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
					}
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