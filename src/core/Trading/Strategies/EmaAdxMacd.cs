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

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> ema4 = candles.Ema(4);
			List<decimal?> ema10 = candles.Ema(10);
			List<decimal?> plusDi = candles.PlusDi(28);
			List<decimal?> minusDi = candles.MinusDi(28);
			MacdItem macd = candles.Macd(5, 10, 4);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (ema4[i] < ema10[i] && ema4[i - 1] > ema10[i] && macd.Macd[i] < 0 && plusDi[i] > minusDi[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (ema4[i] > ema10[i] && ema4[i - 1] < ema10[i] && macd.Macd[i] > 0 && plusDi[i] < minusDi[i])
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
