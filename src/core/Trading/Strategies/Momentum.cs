using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class Momentum : BaseStrategy
	{
		public override string Name { get; } = "Momentum";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 30;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> smaFast = candles.Sma(11);
			List<decimal?> smaSlow = candles.Sma(21);
			List<decimal?> mom = candles.Mom(30);
			List<decimal?> rsi = candles.Rsi();
			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 30 && mom[i] > 0 && smaFast[i] > smaSlow[i] && closes[i] > smaSlow[i] && closes[i] > smaFast[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (rsi[i] > 70 && mom[i] < 0 && smaFast[i] < smaSlow[i] && closes[i] < smaSlow[i] && closes[i] < smaFast[i])
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