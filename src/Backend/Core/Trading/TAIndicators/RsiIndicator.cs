using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
	public class RsiIndicator : BaseIndicator<RsiOptions, DefaultIndicatorResult>
	{
		public override string Name { get; } = "Relative Strength Index (RSI) Indicator";
		
		public override DefaultIndicatorResult Get(ICandle[] source, RsiOptions options)
		{
			decimal?[] values = source.Select(x => (decimal?)x.Close).ToArray();
			return Get(values, options);
		}

		public override DefaultIndicatorResult Get(decimal[] source, RsiOptions options)
		{
			decimal?[] values = source.Select(x => (decimal?)x).ToArray();
			return Get(values, options);
		}

		public override DefaultIndicatorResult Get(decimal?[] source, RsiOptions options)
		{
			decimal?[] rsi = new decimal?[source.Length];
			decimal?[] change = new decimal?[source.Length];

			for (int i = 1; i < source.Length; i++)
			{
				if (i >= options.Period)
				{
					decimal? averageGain = change.Where(x => x > 0).Sum() / change.Length;
					decimal? averageLoss = -1 * change.Where(x => x < 0).Sum() / change.Length;
					
					decimal? rs = averageGain / averageLoss;
					rsi[i] = 100 - (100 / (1 + rs));
				}
				else
				{
					rsi[i] = null;
				}
				
				change[i] = source[i] - source[i - 1];
			}

			return new DefaultIndicatorResult() { Result = rsi };
		}
	}
}