using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;

namespace Core.Trading.Indicators
{
	public class BearBull : BaseIndicator
	{
		public override string Name => nameof(BearBull);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			List<decimal> closes = source.Select(x => Convert.ToDecimal(x.Close)).ToList();
			List<int> result = new List<int>();

			for (int i = 0; i < closes.Count; i++)
			{
				if (i < 2)
				{
					result.Add(0);
				}
				else
				{
					decimal current = closes[i];
					decimal previous = closes[i - 1];
					decimal prior = closes[i - 2];

					if (current > previous && previous > prior)
					{
						result.Add(1); // last two candles were bullish
					}
					else if (current < previous && previous < prior)
					{
						result.Add(-1); // last two candles were bearish
					}
					else
					{
						result.Add(0);
					}
				}
			}

			return result;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
