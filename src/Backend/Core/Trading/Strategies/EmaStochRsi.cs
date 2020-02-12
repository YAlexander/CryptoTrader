using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class EmaStochRsi : BaseStrategy
	{
		public override string Name { get; } = "EMA Stoch RSI";
		
		public override int MinNumberOfCandles { get; } = 36;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			StochItem stoch = candles.Stoch(14);
			List<decimal?> emaShort = candles.Ema(5);
			List<decimal?> emaLong = candles.Ema(10);
			List<decimal?> rsi = candles.Rsi(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					decimal? slowK = stoch.K[i];
					decimal? slowKp = stoch.K[i - 1];
					decimal? slowD = stoch.D[i];
					decimal? slowDp = stoch.D[i - 1];

					bool pointedUp = false, pointedDown = false, kUp = false, dUp = false;

					if (slowKp < slowK)
					{
						kUp = true;
					}

					if (slowDp < slowD)
					{
						dUp = true;
					}

					if (slowKp < 80 && slowDp < 80 && kUp && dUp)
					{
						pointedUp = true;
					}

					if (slowKp > 20 && slowDp > 20 && !kUp && !dUp)
					{
						pointedDown = true;
					}

					if (emaShort[i] >= emaLong[i] && emaShort[i - 1] < emaLong[i] && rsi[i] > 50 && pointedUp)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (emaShort[i] <= emaLong[i] && emaShort[i - 1] > emaLong[i] && rsi[i] < 50 && pointedDown)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}
	}
}
