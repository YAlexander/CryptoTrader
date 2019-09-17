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

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			StochItem stoch = candles.Stoch(14);
			List<decimal?> ema5 = candles.Ema(5);
			List<decimal?> ema10 = candles.Ema(10);
			List<decimal?> rsi = candles.Rsi(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add(TradingAdviceCode.HOLD);
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

					if (ema5[i] >= ema10[i] && ema5[i - 1] < ema10[i] && rsi[i] > 50 && pointedUp)
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (ema5[i] <= ema10[i] && ema5[i - 1] > ema10[i] && rsi[i] < 50 && pointedDown)
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else
					{
						result.Add(TradingAdviceCode.HOLD);
					}
				}
			}

			return result.LastOrDefault();
		}
	}
}
