using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class EmaCross : BaseStrategy
	{
		public override string Name { get; } = "EMA Cross";

		public override int MinNumberOfCandles { get; } = 36;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			EmaCrossOptions options = Options.GetOptions<EmaCrossOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> emaShort = candles.Ema(options?.Ema1 ?? 12);
			List<decimal?> emaLong = candles.Ema(options?.Ema2 ?? 26);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (emaShort[i] < emaLong[i] && emaShort[i - 1] > emaLong[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (emaShort[i] > emaLong[i] && emaShort[i - 1] < emaLong[i])
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
