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

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();
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
						result.Add(TradingAdviceCode.BUY);
					}
					else if (current < previous)
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else
					{
						result.Add(TradingAdviceCode.HOLD);
					}
				}
				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault();
		}
	}
}