using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class SimpleBearBullStrategy : BaseStrategy<EmptyStrategyOptions>
	{
		public override string Name { get; } = "The Bull and The Bear Strategy";
		
		public override int MinNumberOfCandles { get; } = 5;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, null);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();
			decimal[] closes = candles.Close();

			for (int i = 0; i < candles.Length; i++)
			{
				if (i >= 2)
				{
					decimal current = closes[i];
					decimal previous = closes[i - 1];
					decimal prior = closes[i - 2];

					if (current > previous && previous > prior)
					{
						result.Add((candles[i], TradingAdvices.BUY));
					}
					else if (current < previous)
					{
						result.Add((candles[i], TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles[i], TradingAdvices.HOLD));
					}
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public SimpleBearBullStrategy(EmptyStrategyOptions options) : base(options)
		{
		}
	}
}