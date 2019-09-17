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
	public class TheScalper : BaseStrategy
	{
		public override string Name { get; } = "The Scalper";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 200;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			StochItem stoch = candles.Stoch();
			List<decimal?> sma200 = candles.Sma(200);

			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else
				{
					if (sma200[i] < closes[i] &&
						stoch.K[i - 1] <= stoch.D[i - 1] &&
						stoch.K[i] > stoch.D[i] &&
						stoch.D[i - 1] < 20 &&
						stoch.K[i - 1] < 20)
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (stoch.K[i - 1] <= stoch.D[i - 1] &&
						stoch.K[i] > stoch.D[i] &&
						stoch.D[i - 1] > 80 &&
						stoch.K[i - 1] > 80)
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