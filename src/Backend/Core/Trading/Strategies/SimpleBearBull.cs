using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;

namespace Core.Trading.Strategies
{
	public class SimpleBearBull : BaseStrategy
	{
		public override string Name { get; } = "The Bull and The Bear";
		
		public override int MinNumberOfCandles { get; } = 5;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();
			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i >= 2)
				{
					decimal current = closes[i];
					decimal previous = closes[i - 1];
					decimal prior = closes[i - 2];

					if (current > previous && previous > prior)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (current < previous)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
					}
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