using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class SmaCrossoverEvo : BaseStrategy
	{
		public override string Name { get; } = "SMA Crossover EVO";

		public override int MinNumberOfCandles { get; } = 60;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			SmaCrossoverEvoOptions options = Options.GetOptions<SmaCrossoverEvoOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> shorts = candles.Ema(options?.Ema1 ?? 6);
			List<decimal?> longs = candles.Ema(options?.Ema2 ?? 3);

			for (int i = 0; i < candles.Count(); i++)
			{
				var diff = 100 * (shorts[i] - longs[i]) / ((shorts[i] + longs[i]) / 2);

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
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}
			return result;
		}
	}
}
