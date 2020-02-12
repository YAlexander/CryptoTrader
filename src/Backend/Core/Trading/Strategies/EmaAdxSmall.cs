using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class EmaAdxSmall : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX Small";

		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal?> emaFast = candles.Ema(3);
			List<decimal?> emaSlow = candles.Ema(10);
			List<decimal?> minusDi = candles.MinusDi(14);
			List<decimal?> plusDi = candles.PlusDi(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (emaFast[i] > emaSlow[i] && (emaFast[i - 1] < emaSlow[i - 1] || plusDi[i - 1] < minusDi[i - 1]) && plusDi[i] > 20 && plusDi[i] > minusDi[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (emaFast[i] < emaSlow[i] && emaFast[i - 1] > emaSlow[i - 1])
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