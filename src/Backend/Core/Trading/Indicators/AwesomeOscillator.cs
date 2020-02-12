using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Extensions;

namespace Core.Trading.Indicators
{
	public class AwesomeOscillator : BaseIndicator
	{
		public override string Name => nameof(AwesomeOscillator);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			// TODO: Move to options
			bool returnRaw = false;

			// Calculate our Moving Averages
			IEnumerable<decimal?> smaFastData = source.Select(x => (decimal?)((x.High + x.Low) / 2));
			List<decimal?> smaFast = smaFastData.Sma(5);

			IEnumerable<decimal?> smaSlowData = source.Select(x => (decimal?)((x.High + x.Low) / 2));
			List<decimal?> smaSlow = smaFastData.Sma(34);

			List<decimal?> result = new List<decimal?>();

			for (int i = 0; i < smaFast.Count(); i++)
			{
				if (returnRaw)
				{
					if (!smaFast[i].HasValue || !smaSlow[i].HasValue)
					{
						result.Add(null);
					}
					else
					{
						result.Add(smaFast[i].Value - smaSlow[i].Value);
					}
				}
				else
				{
					// The last and second to last values interest us, because we're looking for a cross of these lines.
					// If it's not the first item, we can check the previous.
					if (i > 0)
					{
						decimal? smaFastLast = smaFast[i];
						decimal? smaSlowLast = smaSlow[i];
						decimal? smaFastSecondLast = smaFast[i - 1];
						decimal? smaSlowSecondLast = smaSlow[i - 1];

						decimal? aoSecondLast = smaFastSecondLast - smaSlowSecondLast;
						decimal? aoLast = smaFastLast - smaSlowLast;

						if (aoSecondLast <= 0 && aoLast > 0)
						{
							result.Add(100);
						}
						else if (aoSecondLast >= 0 && aoLast < 0)
						{
							result.Add(-100);
						}
						else
						{
							result.Add(0);
						}
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
			throw new System.NotImplementedException();
		}
	}
}
