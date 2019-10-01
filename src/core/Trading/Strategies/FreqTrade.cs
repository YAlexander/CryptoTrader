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
	public class FreqTrade : BaseStrategy
	{
		public override string Name { get; } = "Freq Trade";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.QUARTER_HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> rsi = candles.Rsi(14);
			List<decimal?> adx = candles.Adx(14);
			List<decimal?> plusDi = candles.PlusDi(14);
			List<decimal?> minusDi = candles.MinusDi(14);
			StochItem fast = candles.StochFast();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 25 && fast.D[i] < 30 && adx[i] > 30 && plusDi[i] > 5)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (adx[i] > 0 && minusDi[i] > 0 && fast.D[i] > 65)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
			}

			return result;
		}
	}
}
