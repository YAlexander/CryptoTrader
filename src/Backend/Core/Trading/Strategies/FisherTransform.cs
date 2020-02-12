using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class FisherTransform : BaseStrategy
	{
		public override string Name { get; } = "Fisher Transform";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			FisherTransformOptions options = Options.GetOptions<FisherTransformOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> fishers = candles.Fisher(options?.Fisher ?? 10);
			List<decimal?> ao = candles.AwesomeOscillator();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (fishers[i] < 0 && fishers[i - 1] > 0 && ao[i] < 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (fishers[i] == 1)
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