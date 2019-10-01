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
	public class PowerRanger : BaseStrategy
	{
		public override string Name { get; } = "Power Ranger";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 10;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();
			StochItem stoch = candles.Stoch(10);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					if (stoch.K[i] > 20 && stoch.K[i - 1] < 20 || stoch.D[i] > 20 && stoch.D[i - 1] < 20)
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (stoch.K[i] < 80 && stoch.K[i - 1] > 80 || stoch.D[i] < 80 && stoch.D[i - 1] > 80)
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
