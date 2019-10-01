using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class CciScalper : BaseStrategy
	{
		public override string Name { get; } = "CCI Scalper";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 14;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> cci = candles.Cci();
			List<decimal?> ema10 = candles.Ema(10);
			List<decimal?> ema21 = candles.Ema(21);
			List<decimal?> ema50 = candles.Ema(50);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (cci[i] < -100 && ema10[i] > ema21[i] && ema10[i] > ema50[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (cci[i] > 100 && ema10[i] < ema21[i] && ema10[i] < ema50[i])
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
