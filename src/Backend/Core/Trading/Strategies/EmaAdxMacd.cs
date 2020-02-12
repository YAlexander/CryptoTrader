using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class EmaAdxMacd : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX MACD";
		
		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> emaShort = candles.Ema(4);
			List<decimal?> emaLong = candles.Ema(10);
			List<decimal?> plusDi = candles.PlusDi(28);
			List<decimal?> minusDi = candles.MinusDi(28);
			MacdItem macd = candles.Macd(5, 10, 4);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (emaShort[i] < emaLong[i] && emaShort[i - 1] > emaLong[i] && macd.Macd[i] < 0 && plusDi[i] > minusDi[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (emaShort[i] > emaLong[i] && emaShort[i - 1] < emaLong[i] && macd.Macd[i] > 0 && plusDi[i] < minusDi[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}
