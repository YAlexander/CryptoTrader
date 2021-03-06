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
	public class EmaStochRsi : BaseStrategy
	{
		public override string Name { get; } = "EMA Stoch RSI";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 36;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			StochItem stoch = candles.Stoch(14);
			List<decimal?> emaShort = candles.Ema(5);
			List<decimal?> emaLong = candles.Ema(10);
			List<decimal?> rsi = candles.Rsi(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					decimal? slowk1 = stoch.K[i];
					decimal? slowkp = stoch.K[i - 1];
					decimal? slowd1 = stoch.D[i];
					decimal? slowdp = stoch.D[i - 1];

					bool pointedUp = false, pointedDown = false, kUp = false, dUp = false;

					if (slowkp < slowk1)
					{
						kUp = true;
					}

					if (slowdp < slowd1)
					{
						dUp = true;
					}

					if (slowkp < 80 && slowdp < 80 && kUp && dUp)
					{
						pointedUp = true;
					}

					if (slowkp > 20 && slowdp > 20 && !kUp && !dUp)
					{
						pointedDown = true;
					}

					if (emaShort[i] >= emaLong[i] && emaShort[i - 1] < emaLong[i] && rsi[i] > 50 && pointedUp)
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (emaShort[i] <= emaLong[i] && emaShort[i - 1] > emaLong[i] && rsi[i] < 50 && pointedDown)
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
