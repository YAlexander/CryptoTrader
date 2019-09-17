using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class AdxSmas : BaseStrategy
	{
		public override string Name { get; } = "ADX Smas";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 14;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sma6 = candles.Sma(3);
			List<decimal?> sma40 = candles.Sma(10);
			List<decimal?> adx = candles.Adx(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else
				{
					int sixCross = sma6[i - 1] < sma40[i] && sma6[i] > sma40[i] ? 1 : 0;
					int fortyCross = sma40[i - 1] < sma6[i] && sma40[i] > sma6[i] ? 1 : 0;

					if (adx[i] > 25 && sixCross == 1)
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (adx[i] < 25 && fortyCross == 1)
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
