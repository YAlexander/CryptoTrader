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
	public class FreqTradeEvo : BaseStrategy
	{
		public override string Name { get; } = "Freq Trade Evo";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.QUARTER_HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> rsi = candles.Rsi(5);
			StochItem fast = candles.StochFast();
			BbandItem bb = candles.Bbands(20);

			List<decimal?> adx = candles.Adx(14);
			List<decimal?> plusDi = candles.PlusDi(14);
			List<decimal?> minusDi = candles.MinusDi(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 22 && fast.K[i] < 25 && fast.D[i - 1] > fast.K[i - 1] && fast.D[i] - fast.K[i] < 0.3m)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (rsi[i] > 70 && fast.K[i] > 50)
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