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
	public class MacdTema : BaseStrategy
	{
		public override string Name { get; } = "MACD TEMA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 50;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();
			MacdItem macd = candles.Macd(12, 26, 9);
			List<decimal?> tema = candles.Tema(50);

			List<decimal> close = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (tema[i] < close[i] && tema[i - 1] > close[i - 1] && macd.Macd[i] > 0 && macd.Macd[i - 1] < 0)
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (tema[i] > close[i] && tema[i - 1] < close[i - 1] && macd.Macd[i] < 0 && macd.Macd[i - 1] > 0)
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