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

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> smaShort = candles.Sma(3);
			List<decimal?> smaLong = candles.Sma(10);
			List<decimal?> adx = candles.Adx(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					int sixCross = smaShort[i - 1] < smaLong[i] && smaShort[i] > smaLong[i] ? 1 : 0;
					int fortyCross = smaLong[i - 1] < smaShort[i] && smaLong[i] > smaShort[i] ? 1 : 0;

					if (adx[i] > 25 && sixCross == 1)
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (adx[i] < 25 && fortyCross == 1)
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
