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
	public class EmaAdxMacd : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX MACD";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 30;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> emaShort = candles.Ema(4);
			List<decimal?> emaLong = candles.Ema(10);
			List<decimal?> plusDi = candles.PlusDi(28);
			List<decimal?> minusDi = candles.MinusDi(28);
			MacdItem macd = candles.Macd(5, 10, 4);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (emaShort[i] < emaLong[i] && emaShort[i - 1] > emaLong[i] && macd.Macd[i] < 0 && plusDi[i] > minusDi[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (emaShort[i] > emaLong[i] && emaShort[i - 1] < emaLong[i] && macd.Macd[i] > 0 && plusDi[i] < minusDi[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}
	}
}
