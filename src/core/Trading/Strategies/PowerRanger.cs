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

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();
			StochItem stoch = candles.Stoch(10);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else
				{
					if (stoch.K[i] > 20 && stoch.K[i - 1] < 20 || stoch.D[i] > 20 && stoch.D[i - 1] < 20)
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (stoch.K[i] < 80 && stoch.K[i - 1] > 80 || stoch.D[i] < 80 && stoch.D[i - 1] > 80)
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else
					{
						result.Add(TradingAdviceCode.HOLD);
					}
				}
			}

			return result.LastOrDefault();
		}
	}
}
