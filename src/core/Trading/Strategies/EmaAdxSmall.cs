using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class EmaAdxSmall : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX Small";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 15;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal?> emaFast = candles.Ema(3);
			List<decimal?> emaSlow = candles.Ema(10);
			List<decimal?> minusDi = candles.MinusDi(14);
			List<decimal?> plusDi = candles.PlusDi(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (emaFast[i] > emaSlow[i] && (emaFast[i - 1] < emaSlow[i - 1] || plusDi[i - 1] < minusDi[i - 1]) && plusDi[i] > 20 && plusDi[i] > minusDi[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (emaFast[i] < emaSlow[i] && emaFast[i - 1] > emaSlow[i - 1])
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