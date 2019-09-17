using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.TypeCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.Trading.Strategies
{
	public class MacdSma : BaseStrategy
	{
		public override string Name { get; } = "MACD SMA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 200;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			MacdItem macd = candles.Macd();
			List<decimal?> fastMa = candles.Sma(12);
			List<decimal?> slowMa = candles.Sma(26);
			List<decimal?> sma200 = candles.Sma(200);

			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 25)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (slowMa[i] < sma200[i])
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (macd.Hist[i] > 0 && macd.Hist[i - 1] < 0 && macd.Macd[i] > 0 && fastMa[i] > slowMa[i] && closes[i - 26] > sma200[i])
				{
					result.Add(TradingAdviceCode.BUY);
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
