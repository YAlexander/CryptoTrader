using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class BigThree : BaseStrategy
	{
		public override string Name { get; } = "Big Three";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 100;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> sma20 = candles.Sma(20);
			List<decimal?> sma40 = candles.Sma(40);
			List<decimal?> sma80 = candles.Sma(80);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 2)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					ICandle[] candlesArray = candles.ToArray();

					bool lastIsGreen = candlesArray[i].Close > candlesArray[i].Open;
					bool previousIsRed = candlesArray[i - 1].Close < candlesArray[i - 1].Open;
					bool beforeIsGreen = candlesArray[i - 2].Close > candlesArray[i - 2].Open;

					decimal? highestSma = new List<decimal?> { sma20[i], sma40[i], sma80[i] }.Max();

					bool lastAboveSma = candlesArray[i].Close > highestSma && candlesArray[i].High > highestSma && candlesArray[i].Low > highestSma && candlesArray[i].Open > highestSma;

					bool previousAboveSma = candlesArray[i - 1].Close > highestSma && candlesArray[i - 1].High > highestSma && candlesArray[i - 1].Low > highestSma && candlesArray[i - 1].Open > highestSma;

					bool beforeAboveSma = candlesArray[i - 2].Close > highestSma && candlesArray[i - 2].High > highestSma && candlesArray[i - 2].Low > highestSma && candlesArray[i - 2].Open > highestSma;

					bool allAboveSma = lastAboveSma && previousAboveSma && beforeAboveSma;
					bool hitsAnSma = sma80[i] < candlesArray[i].High && sma80[i] > candlesArray[i].Low;

					if (lastIsGreen && previousIsRed && beforeIsGreen && allAboveSma && sma20[i] > sma40[i] && sma20[i] > sma80[i])
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (hitsAnSma)
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
