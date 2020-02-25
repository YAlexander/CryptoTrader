using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class Base150 : BaseStrategy
	{
		public override string Name { get; } = "Base 150";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 365;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> sma6 = candles.Sma(6);
			List<decimal?> sma25 = candles.Sma(25);
			List<decimal?> sma150 = candles.Sma(150);
			List<decimal?> sma365 = candles.Sma(365);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					if (sma6[i] > sma150[i] && sma6[i] > sma365[i] && sma25[i] > sma150[i] && sma25[i] > sma365[i] && (sma6[i - 1] < sma150[i] || sma6[i - 1] < sma365[i] || sma25[i - 1] < sma150[i] || sma25[i - 1] < sma365[i]))
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					if (sma6[i] < sma150[i] && sma6[i] < sma365[i] && sma25[i] < sma150[i] && sma25[i] < sma365[i] && (sma6[i - 1] > sma150[i] || sma6[i - 1] > sma365[i] || sma25[i - 1] > sma150[i] || sma25[i - 1] > sma365[i]))
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
					}
				}
			}

			return result;
		}
	}
}
