using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class EmaCrossShort : BaseStrategy
	{
		public override string Name { get; } = "EMA Cross Short";

		public override int MinNumberOfCandles { get; } = 36;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			EmaCrossShortOptions options = Options.GetOptions<EmaCrossShortOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> shorts = candles.Ema(options?.EmaShort ?? 6);
			List<decimal?> longs = candles.Ema(options?.EmaLong ?? 3);

			for (int i = 0; i < candles.Count(); i++)
			{
				decimal? diff = 100 * (shorts[i] - longs[i]) / ((shorts[i] + longs[i]) / 2);

				if (diff > 1.5m)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (diff <= -0.1m)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
			}

			return result;
		}
	}
}
