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
	public class FaMaMaMa : BaseStrategy
	{
		public override string Name { get; } = "FAMAMAMA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 20;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			MamaItem mama = candles.Mama(0.5, 0.05);
			MamaItem fama = candles.Mama(0.25, 0.025);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (fama.Mama[i] > mama.Mama[i] && fama.Mama[i - 1] < mama.Mama[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (fama.Mama[i] < mama.Mama[i] && fama.Mama[i - 1] > mama.Mama[i])
				{
					result.Add(TradingAdviceCode.SELL);
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